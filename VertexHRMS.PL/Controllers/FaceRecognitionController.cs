using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using VertexHRMS.BLL.ModelVM.AttendanceRecords;
using VertexHRMS.BLL.ModelVM.FaceRecognition;
using VertexHRMS.BLL.Service.Abstraction;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.PL.Controllers
{
    [Authorize(Roles = "Employee")]
    public class FaceRecognitionController : Controller
    {
        private readonly IFaceRecognitionService _faceRecognition;
        private readonly IAttendanceRecordsService _attendanceRecordsService;
        private readonly ILogger<FaceRecognitionController> _logger;

        public FaceRecognitionController(IFaceRecognitionService faceRecognition,
                                         IAttendanceRecordsService attendanceRecordsService,
                                         ILogger<FaceRecognitionController> logger)
        {
            _faceRecognition = faceRecognition;
            _attendanceRecordsService = attendanceRecordsService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult FaceLogin()
        {
            return View();
        }

        [HttpPost]
        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> FaceLogin(IFormFile file, string status)
        {
            try
            {
                if (file == null || file.Length == 0)
                {
                    _logger.LogWarning("No image uploaded in FaceLogin request");
                    return Json(new { success = false, message = "No image uploaded" });
                }

                // Save uploaded image temporarily
                var tempFile = Path.Combine(Path.GetTempPath(), Guid.NewGuid() + "_snapshot.jpg");
                using (var stream = new FileStream(tempFile, FileMode.Create))
                    await file.CopyToAsync(stream);

                _logger.LogDebug("Saved uploaded snapshot to {TempFile}", tempFile);

                Employee? result = null;
                try
                {
                    result = await _faceRecognition.Recognize(tempFile);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Recognition failed for uploaded image {TempFile}", tempFile);
                    result = null;
                }

                if (result != null)
                {
                    try
                    {
                        var record = new AttendanceRecordsVM
                        {
                            EmployeeId = result.EmployeeId,
                            CheckIn = DateTime.Now,
                            Status = status
                        };
                        await _attendanceRecordsService.AddAttendanceRecordAsync(record);
                        _logger.LogInformation("Attendance added for employee {Id}", result.EmployeeId);
                    }
                    catch (Exception ex)
                    {
                        // TEMPORARY: return full exception for debugging (remove in production)
                        _logger.LogError(ex, "Failed to add attendance for employee {Id}", result.EmployeeId);
                        return Json(new { success = false, message = "Failed to save attendance: " + ex.ToString() });
                    }

                    var vm = new FaceLoginVM
                    {
                        EmpId = result.EmployeeId,
                        EmpName = (result.FirstName ?? string.Empty) + " " + (result.LastName ?? string.Empty),
                        DeptName = result.Department?.DepartmentName ?? string.Empty,
                        PosName = result.Position?.PositionName ?? string.Empty,
                    };

                    _logger.LogDebug("Returning FaceLogin VM: {@Vm}", vm);
                    return Json(new { success = true, emp = vm });
                }

                _logger.LogInformation("Face recognition returned no match for uploaded image {TempFile}", tempFile);
                return Json(new { success = false, message = "No match found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FaceLogin unexpected error");
                return Json(new { success = false, message = ex.Message });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Checkout([FromBody] int employeeId)
        {
            try
            {
                var record = await _attendanceRecordsService.CheckoutAsync(employeeId);
                if (record != null)
                {
                    return Json(new { success = true, message = $"Checked out at {record.CheckOut}" });
                }

                return Json(new { success = false, message = "No active check-in found" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Checkout error for employee {Id}", employeeId);
                return Json(new { success = false, message = "No active check-in found" });
            }
        }
    }
}
