using Microsoft.Extensions.Logging;
using System.Net.Http.Headers;
using System.Text.Json;
using VertexHRMS.BLL.Services.Abstraction;

namespace VertexHRMS.BLL.Services.Implementation
{
    /// <summary>
    /// Affinda-backed resume parser.
    /// POST /v3/documents?wait=true&document_type=resume
    /// Authorization: Bearer {apiKey}
    /// </summary>
    public class SimpleResumeParser : IResumeParser
    {
        private readonly HttpClient _http;
        private readonly string _apiKey;
        private readonly JsonSerializerOptions _json;
        private readonly ILogger<SimpleResumeParser> _log;

        public SimpleResumeParser(HttpClient http, IConfiguration cfg, ILogger<SimpleResumeParser> log)
        {
            _http = http;
            _log = log;

            var baseUrl = cfg["ATS:BaseUrl"] ?? "https://api.affinda.com";
            _http.BaseAddress = new Uri(baseUrl);

            _apiKey = cfg["ATS:AffindaApiKey"] ?? throw new InvalidOperationException("ATS:AffindaApiKey is missing.");
            _http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);

            _json = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
        }

        public async Task<ResumeParseResult> ParseAsync(string filePath, CancellationToken ct = default)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("Resume file not found.", filePath);

            // Build multipart for the upload
            using var form = new MultipartFormDataContent();
            var fileName = Path.GetFileName(filePath);
            var stream = File.OpenRead(filePath);
            var fileContent = new StreamContent(stream);
            fileContent.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            form.Add(fileContent, "file", fileName);

            // Ask Affinda to parse synchronously (‘wait=true’) as a resume document
            var url = "/v3/documents?wait=true&document_type=resume";
            using var resp = await _http.PostAsync(url, form, ct);
            var body = await resp.Content.ReadAsStringAsync(ct);

            if (!resp.IsSuccessStatusCode)
            {
                _log.LogWarning("Affinda parse failed: {Status} {Body}", (int)resp.StatusCode, body);
                // Return a safe fallback so the app doesn’t crash
                return new ResumeParseResult(
                    name: "Unknown",
                    email: "",
                    skillsCsv: "",
                    years: 0
                );
            }

            // Minimal, defensive JSON mapping (Affinda v3 shape)
            // We expect something like:
            // { "data": { "name": { "raw": "..." }, "emails": ["..."], "skills": [{ "name": "C#" }, ...], "work_experience": [{ ... }] } }
            try
            {
                using var doc = JsonDocument.Parse(body);

                var data = doc.RootElement.GetProperty("data");

                string name = TryGetString(data, "name", "raw")
                              ?? TryGetString(data, "name") ?? "Unknown";

                string email = TryGetFirstString(data, "emails") ?? "";

                string skillsCsv = TryGetArray(data, "skills")
                    ?.Select(e => TryGetString(e, "name") ?? e.GetString())
                    .Where(s => !string.IsNullOrWhiteSpace(s))
                    .Distinct(StringComparer.OrdinalIgnoreCase)
                    .ToCsv()
                    ?? "";

                int years = EstimateYears(data);

                return new ResumeParseResult(name, email, skillsCsv, years);
            }
            catch (Exception ex)
            {
                _log.LogError(ex, "Failed to parse Affinda JSON");
                return new ResumeParseResult("Unknown", "", "", 0);
            }
        }

        // ---- helpers ---------------------------------------------------------

        private static string? TryGetString(JsonElement obj, string prop, string? nested = null)
        {
            if (!obj.TryGetProperty(prop, out var el)) return null;
            if (nested is null)
                return el.ValueKind == JsonValueKind.String ? el.GetString() : null;

            if (el.ValueKind == JsonValueKind.Object && el.TryGetProperty(nested, out var inner) && inner.ValueKind == JsonValueKind.String)
                return inner.GetString();

            return null;
        }

        private static IEnumerable<JsonElement>? TryGetArray(JsonElement obj, string prop)
        {
            return obj.TryGetProperty(prop, out var el) && el.ValueKind == JsonValueKind.Array
                ? el.EnumerateArray()
                : null;
        }

        private static string? TryGetFirstString(JsonElement obj, string prop)
        {
            var arr = TryGetArray(obj, prop);
            if (arr is null) return null;
            foreach (var e in arr)
            {
                if (e.ValueKind == JsonValueKind.String) return e.GetString();
            }
            return null;
        }

        private static int EstimateYears(JsonElement data)
        {
            // Best-effort: if Affinda provides total years, prefer that.
            // Otherwise derive from work_experience dates.
            try
            {
                // Some responses may include "total_years_experience"
                if (data.TryGetProperty("total_years_experience", out var yrs)
                    && yrs.ValueKind == JsonValueKind.Number
                    && yrs.TryGetInt32(out var intYears))
                    return intYears;

                if (!data.TryGetProperty("work_experience", out var we) || we.ValueKind != JsonValueKind.Array)
                    return 0;

                DateTime? minStart = null, maxEnd = null;

                foreach (var item in we.EnumerateArray())
                {
                    DateTime? start = TryParseDate(item, "date_start") ?? TryParseDate(item, "start_date");
                    DateTime? end = TryParseDate(item, "date_end") ?? TryParseDate(item, "end_date");

                    if (start.HasValue)
                        minStart = !minStart.HasValue || start < minStart ? start : minStart;

                    if (end.HasValue)
                        maxEnd = !maxEnd.HasValue || end > maxEnd ? end : maxEnd;
                }

                if (minStart.HasValue)
                {
                    var effectiveEnd = maxEnd ?? DateTime.UtcNow;
                    var years = (effectiveEnd - minStart.Value).TotalDays / 365.25;
                    return Math.Max(0, (int)Math.Round(years, MidpointRounding.AwayFromZero));
                }
            }
            catch { /* swallow & return 0 */ }

            return 0;
        }

        private static DateTime? TryParseDate(JsonElement obj, string prop)
        {
            if (!obj.TryGetProperty(prop, out var el)) return null;
            if (el.ValueKind == JsonValueKind.String && DateTime.TryParse(el.GetString(), out var dt))
                return dt;
            return null;
        }
    }

    internal static class CsvExtensions
    {
        public static string ToCsv(this IEnumerable<string> values) =>
            string.Join(", ", values.Where(v => !string.IsNullOrWhiteSpace(v)).Select(v => v.Trim()));
    }
}
