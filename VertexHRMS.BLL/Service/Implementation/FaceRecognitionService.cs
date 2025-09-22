using FaceRecognitionDotNet;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading;
using System.Threading.Tasks;
using VertexHRMS.BLL.Helper;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class FaceRecognitionService : IFaceRecognitionService, IDisposable
    {
        private readonly FaceRecognition _faceRecognition;
        private readonly string _modelPath;
        private readonly string _filesPath;
        private readonly Dictionary<int, (Employee emp, FaceEncoding encoding)> _encodingsCache = new();
        private readonly ILogger<FaceRecognitionService> _logger;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly CancellationTokenSource _cts = new();
        private Task? _backgroundTask;
        private volatile bool _cacheReady = false;

        // NEW: lock to protect native FaceRecognition/Dlib calls and file write/read races
        private readonly object _faceLock = new();
        private readonly object _fileLock = new();

        public FaceRecognitionService(IHostEnvironment env, IServiceScopeFactory scopeFactory, ILogger<FaceRecognitionService> logger)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;

            _modelPath = Path.Combine(env.ContentRootPath, "wwwroot", "Models");
            _filesPath = Path.Combine(env.ContentRootPath, "wwwroot", "Files");

            string modelFile = Path.Combine(_modelPath, "dlib_face_recognition_resnet_model_v1.dat");
            if (!File.Exists(modelFile))
            {
                _logger.LogError("Face recognition model not found at {ModelFile}", modelFile);
                throw new FileNotFoundException("Face recognition model not found", modelFile);
            }

            _logger.LogInformation("Initializing FaceRecognition (model folder: {ModelPath})", _modelPath);

            // Create face recognition instance (single instance, but we'll synchronize access)
            _faceRecognition = FaceRecognition.Create(_modelPath);

            // Ensure Processed folder exists
            Directory.CreateDirectory(Path.Combine(_filesPath, "Processed"));

            // Start background cache builder (non-blocking)
            _backgroundTask = Task.Run(() => BuildCacheInBackgroundAsync(_cts.Token));
        }

        private async Task BuildCacheInBackgroundAsync(CancellationToken token)
        {
            _logger.LogInformation("FaceRecognitionService: background cache build started");
            try
            {
                using var scope = _scopeFactory.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<VertexHRMSDbContext>();

                // load employees with joins if needed
                var employees = await db.Employees
                                        .AsNoTracking()
                                        .Include(e => e.Department)
                                        .Include(e => e.Position)
                                        .ToListAsync(token);

                _logger.LogDebug("Background: loaded {Count} employees from DB", employees.Count);

                // جوه BuildCacheInBackgroundAsync
                foreach (var emp in employees)
                {
                    if (token.IsCancellationRequested) break;
                    if (string.IsNullOrWhiteSpace(emp.ImagePath))
                    {
                        _logger.LogDebug("Employee {Id} skipped: empty ImagePath", emp.EmployeeId);
                        continue;
                    }

                    var rawPath = emp.ImagePath.Trim();
                    var relativePath = rawPath.TrimStart('/', '\\', '~').Trim();
                    if (relativePath.StartsWith("wwwroot", StringComparison.OrdinalIgnoreCase))
                        relativePath = relativePath.Substring("wwwroot".Length).TrimStart('/', '\\');

                    if (relativePath.StartsWith("Files", StringComparison.OrdinalIgnoreCase))
                        relativePath = relativePath.Substring("Files".Length).TrimStart('/', '\\');

                    var absolutePath = Path.GetFullPath(Path.Combine(_filesPath, relativePath));

                    _logger.LogDebug("Employee {Id} image raw='{Raw}', relative='{Relative}', resolved='{Absolute}'",
                                     emp.EmployeeId, rawPath, relativePath, absolutePath);

                    if (!File.Exists(absolutePath))
                    {
                        _logger.LogWarning("Employee {Id} image not found at {Path}", emp.EmployeeId, absolutePath);
                        continue;
                    }

                    var processedPath = Path.Combine(_filesPath, "Processed", $"{emp.EmployeeId}.jpg");

                    try
                    {
                        lock (_fileLock)
                        {
                            ImageHelper.ResizeImage(absolutePath, processedPath, 400);
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to resize/process image for Employee {Id} from {Path}", emp.EmployeeId, absolutePath);
                        continue;
                    }

                    FaceEncoding? encoding = null;
                    try
                    {
                        encoding = EncodeImage(processedPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Failed to encode image for Employee {Id} at {Path}", emp.EmployeeId, processedPath);
                    }

                    if (encoding != null)
                    {
                        lock (_encodingsCache)
                        {
                            _encodingsCache[emp.EmployeeId] = (emp, encoding);
                        }
                        _logger.LogInformation("✅ Cached encoding for Employee {Id}", emp.EmployeeId);
                    }
                    else
                    {
                        _logger.LogWarning("⚠️ No valid encoding for Employee {Id} at {Path}", emp.EmployeeId, processedPath);
                    }
                }


                _logger.LogInformation("Background cache build finished. Encodings count: {Count}", _encodingsCache.Count);
                _cacheReady = true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Background cache build failed");
            }
        }

        public FaceEncoding? EncodeImage(string imagePath)
        {
            try
            {
                _logger.LogDebug("Encoding image {Path}", imagePath);

                if (!File.Exists(imagePath))
                {
                    _logger.LogWarning("EncodeImage: file not found {Path}", imagePath);
                    return null;
                }

                lock (_faceLock)
                {
                    using var image = FaceRecognition.LoadImageFile(imagePath);
                    var encs = _faceRecognition.FaceEncodings(image).ToList();
                    _logger.LogDebug("Found {Count} encodings in {Path}", encs.Count, imagePath);

                    if (encs.Count != 1) return null;
                    return encs[0];
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "EncodeImage failed for {Path}", imagePath);
                return null;
            }
        }

        public async Task<Employee?> Recognize(string imagePath, double tolerance = 0.6)
        {
            _logger.LogDebug("Recognize called for {Path} (cacheReady={CacheReady})", imagePath, _cacheReady);

            // make a processed temporary file for the uploaded image to match pipeline
            string tempProcessed = Path.Combine(Path.GetTempPath(), $"uploaded_{Guid.NewGuid()}.jpg");
            try
            {
                // if caller provided raw path, make sure it exists
                if (!File.Exists(imagePath))
                {
                    _logger.LogWarning("Recognize: uploaded image not found {Path}", imagePath);
                    return null;
                }

                // resize uploaded to temp processed (protect with fileLock)
                lock (_fileLock)
                {
                    ImageHelper.ResizeImage(imagePath, tempProcessed, 400);
                }

                var uploadedEncoding = EncodeImage(tempProcessed);
                if (uploadedEncoding == null)
                {
                    _logger.LogWarning("Uploaded image failed encoding: {Path}", imagePath);
                    return null;
                }

                List<(Employee emp, FaceEncoding encoding)> snapshot;
                lock (_encodingsCache)
                {
                    snapshot = _encodingsCache.Values.ToList();
                }

                if (snapshot.Count == 0)
                {
                    _logger.LogInformation("Cache empty — attempting quick scan of DB images one-by-one (may be slow)...");
                    try
                    {
                        using var scope = _scopeFactory.CreateScope();
                        var db = scope.ServiceProvider.GetRequiredService<VertexHRMSDbContext>();
                        var employees = await db.Employees
                                                .AsNoTracking()
                                                .Include(e => e.Department)
                                                .Include(e => e.Position)
                                                .ToListAsync();

                        foreach (var emp in employees)
                        {
                            if (string.IsNullOrEmpty(emp.ImagePath)) continue;

                            var rawPath = emp.ImagePath ?? string.Empty;
                            var relativePath = rawPath.TrimStart('/', '\\');
                            var absolutePath = Path.Combine(_filesPath, relativePath);

                            if (!File.Exists(absolutePath))
                            {
                                _logger.LogWarning($"❌ Image not found for EmpId={emp.EmployeeId}, Path={absolutePath}");
                                continue;
                            }

                            var encoding = EncodeImage(absolutePath);
                            if (encoding != null)
                            {
                                _logger.LogInformation($"✅ Cached encoding for EmpId={emp.EmployeeId}, File={relativePath}");
                                _encodingsCache[emp.EmployeeId] = (emp, encoding);
                            }
                            else
                            {
                                _logger.LogWarning($"⚠️ Failed to encode image for EmpId={emp.EmployeeId}, File={relativePath}");
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Quick DB scan failed");
                    }

                    _logger.LogInformation("Quick scan complete - no match");
                    return null;
                }

                // Compare to cached encodings snapshot
                foreach (var kv in snapshot)
                {
                    double distance;
                    try
                    {
                        // FaceDistance is static and safe to call
                        distance = FaceRecognition.FaceDistance(uploadedEncoding, kv.encoding);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogWarning(ex, "FaceDistance failed for emp {Id}", kv.emp.EmployeeId);
                        continue;
                    }

                    _logger.LogDebug("Compare uploaded -> emp {Id}: distance={Distance}", kv.emp.EmployeeId, distance);
                    if (distance < tolerance)
                    {
                        _logger.LogInformation("Match found: Employee {Id} (distance={Distance})", kv.emp.EmployeeId, distance);
                        return kv.emp;
                    }
                }

                _logger.LogInformation("No match found for {Path}", imagePath);
                return null;
            }
            finally
            {
                try { if (File.Exists(tempProcessed)) File.Delete(tempProcessed); } catch { }
            }
        }

        public void Dispose()
        {
            try
            {
                _cts.Cancel();
                _backgroundTask?.Wait(2000);
            }
            catch { }
            try { _faceRecognition?.Dispose(); } catch { }
            _cts.Dispose();
        }
    }
}
