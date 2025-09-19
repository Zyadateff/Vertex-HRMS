namespace VertexHRMS.BLL.Services.Implementation
{
    public class AIService : IAIService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly VertexHRMSDbContext _db;  // Add DbContext

        public AIService(IConfiguration config, HttpClient httpClient, VertexHRMSDbContext db)
        {
            _httpClient = httpClient;
            _apiKey = config["Gemini:ApiKey"] ?? throw new ArgumentNullException("Gemini:ApiKey");
            _db = db;
        }

        public async Task<AIVM> GetAnswerAsync(string question)
        {
            question = (question ?? string.Empty).Trim();
            if (question.Length == 0)
                return new AIVM { Question = question, Answer = "Please enter a question." };

            if (question.Length > 2000)
                question = question.Substring(0, 2000);

            string dbContextText = await GetDatabaseContextAsync(question);

            var context = $@"
            You are a friendly AI assistant that helps explain VERTEX's HRMS (Human Resource Management System) project. 
            Your job is to answer questions in a clear but friendly way. 
            
            Use emojis a little 😊 and keep it professional but approachable.  

            The HRMS project includes: Employee management, Attendance tracking, Payroll, Face recognition login, Role-based access, Multi-language support, Performance evaluation, AI assistant, Recruitment, Onboarding, Profile pages, Department cards, Calendar integration.

            Rules:
            - If the user asks about something not related to the HRMS project or DB, respond:
              'Sorry 😊 I can only answer questions about the HRMS project or available database records.'
            - Use the database context below to answer if relevant.

            Database context:
            {dbContextText}

            User Question: {question}
            ";

            var requestBody = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = context } } }
                }
            };

            try
            {
                using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"https://generativelanguage.googleapis.com/v1beta/models/gemini-1.5-flash:generateContent?key={_apiKey}"
                )
                {
                    Content = JsonContent.Create(requestBody)
                };

                using var response = await _httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return new AIVM
                    {
                        Question = question,
                        Answer = "AI service is currently unavailable. Please try again later."
                    };
                }

                try
                {
                    using var doc = JsonDocument.Parse(responseContent);
                    var root = doc.RootElement;

                    if (root.TryGetProperty("candidates", out var candidates)
                        && candidates.ValueKind == JsonValueKind.Array
                        && candidates.GetArrayLength() > 0)
                    {
                        var first = candidates[0];

                        if (first.TryGetProperty("content", out var contentEl)
                            && contentEl.TryGetProperty("parts", out var parts)
                            && parts.ValueKind == JsonValueKind.Array
                            && parts.GetArrayLength() > 0)
                        {
                            var part = parts[0];
                            if (part.TryGetProperty("text", out var textEl))
                            {
                                var answerText = textEl.GetString() ?? "I couldn't find a clear answer.";
                                return new AIVM { Question = question, Answer = answerText };
                            }
                        }
                    }
                }
                catch (JsonException)
                {
                }

                return new AIVM
                {
                    Question = question,
                    Answer = "I couldn't understand the AI response. Please try again."
                };
            }
            catch (HttpRequestException)
            {
                return new AIVM
                {
                    Question = question,
                    Answer = "Network error while contacting the AI service. Please check your connection."
                };
            }
            catch (Exception)
            {
                return new AIVM
                {
                    Question = question,
                    Answer = "An unexpected error occurred. Please try again later."
                };
            }
        }
        private async Task<string> GetDatabaseContextAsync(string question)
        {
            var sb = new StringBuilder();

            // ---------- Employees ----------
            var employees = await _db.Employees
                .OrderByDescending(e => e.EmployeeId)
                .Take(35)
                .Select(e => new
                {
                    e.EmployeeId,
                    e.EmployeeCode,
                    e.FirstName,
                    e.LastName,
                    e.Email,
                    e.Phone,
                    e.HireDate,
                    e.EmploymentType,
                    e.Status,
                    e.DepartmentId,
                    e.PositionID,
                    ManagerId = e.ManagerId
                })
                .ToListAsync();

            if (employees.Any())
            {
                sb.AppendLine("=== Employees ===");
                foreach (var e in employees)
                {
                    sb.AppendLine(
                        $"Id:{e.EmployeeId}, Code:{e.EmployeeCode}, Name:{e.FirstName} {e.LastName}, Email:{e.Email ?? "-"}, Phone:{e.Phone ?? "-"}, Hire:{e.HireDate:yyyy-MM-dd}, Type:{e.EmploymentType}, Status:{e.Status}, Dept:{e.DepartmentId}, Pos:{e.PositionID}, Manager:{(e.ManagerId.HasValue ? e.ManagerId.Value.ToString() : "NULL")}"
                    );
                }
                sb.AppendLine();
            }

            // ---------- Departments ----------
            var departments = await _db.Departments
                .OrderBy(d => d.DepartmentId)
                .Take(35)
                .Select(d => new { d.DepartmentId, d.DepartmentName, ParentId = d.ParentDepartmentId })
                .ToListAsync();

            if (departments.Any())
            {
                sb.AppendLine("=== Departments ===");
                foreach (var d in departments)
                    sb.AppendLine($"Id:{d.DepartmentId}, Name:{d.DepartmentName}, ParentId:{(d.ParentId.HasValue ? d.ParentId.Value.ToString() : "NULL")}");
                sb.AppendLine();
            }

            // ---------- Positions ----------
            var positions = await _db.Positions
                .OrderBy(p => p.PositionId)
                .Take(35)
                .Select(p => new { p.PositionId, p.PositionName, p.BaseSalary })
                .ToListAsync();

            if (positions.Any())
            {
                sb.AppendLine("=== Positions ===");
                foreach (var p in positions)
                    sb.AppendLine($"Id:{p.PositionId}, Name:{p.PositionName}, BaseSalary:{p.BaseSalary}");
                sb.AppendLine();
            }

            // ---------- WorkSchedules ----------
            var schedules = await _db.WorkSchedules
                .OrderBy(ws => ws.WorkScheduleId)
                .Take(35)
                .Select(ws => new { ws.WorkScheduleId, ws.Name, ws.StartTime, ws.EndTime })
                .ToListAsync();

            if (schedules.Any())
            {
                sb.AppendLine("=== WorkSchedules ===");
                foreach (var s in schedules)
                    sb.AppendLine($"Id:{s.WorkScheduleId}, Name:{s.Name}, {s.StartTime} - {s.EndTime}");
                sb.AppendLine();
            }

            // ---------- Applicants ----------
            var applicants = await _db.Applicants
                .OrderByDescending(a => a.ApplicantId)
                .Take(35)
                .Select(a => new { a.ApplicantId, a.JobOpeningId, a.FirstName, a.LastName, a.Email, a.AppliedDate, a.Status })
                .ToListAsync();

            if (applicants.Any())
            {
                sb.AppendLine("=== Applicants ===");
                foreach (var a in applicants)
                    sb.AppendLine($"Id:{a.ApplicantId}, JobOpening:{a.JobOpeningId}, Name:{a.FirstName} {a.LastName}, Email:{a.Email}, Applied:{a.AppliedDate:yyyy-MM-dd}, Status:{a.Status}");
                sb.AppendLine();
            }

            // ---------- JobOpenings ----------
            var jobOpenings = await _db.JobOpenings
                .OrderByDescending(j => j.JobOpeningId)
                .Take(35)
                .Select(j => new { j.JobOpeningId, j.PositionId, j.DepartmentId, j.JobTitle, j.PostedDate, j.ClosingDate, j.Status })
                .ToListAsync();

            if (jobOpenings.Any())
            {
                sb.AppendLine("=== JobOpenings ===");
                foreach (var j in jobOpenings)
                    sb.AppendLine($"Id:{j.JobOpeningId}, Pos:{j.PositionId}, Dept:{j.DepartmentId}, Title:{j.JobTitle}, Posted:{j.PostedDate:yyyy-MM-dd}, Closes:{j.ClosingDate:yyyy-MM-dd}, Status:{j.Status}");
                sb.AppendLine();
            }

            // ---------- Interviews ----------
            var interviews = await _db.Interviews
                .OrderByDescending(i => i.InterviewId)
                .Take(35)
                .Select(i => new
                {
                    i.InterviewId,
                    i.ApplicantId,
                    i.JobOpeningId,
                    i.InterviewDate,
                    InterviewerId = i.InterviewerId,
                    Feedback = i.Feedback,
                    InterviewerUserId = i.InterviewerUserId
                })
                .ToListAsync();

            if (interviews.Any())
            {
                sb.AppendLine("=== Interviews ===");
                foreach (var it in interviews)
                    sb.AppendLine($"Id:{it.InterviewId}, Applicant:{it.ApplicantId}, JobOpening:{it.JobOpeningId}, Date:{it.InterviewDate:yyyy-MM-dd}, InterviewerEmployee:{(it.InterviewerId.HasValue ? it.InterviewerId.Value.ToString() : "NULL")}, InterviewerUser:{it.InterviewerUserId ?? "NULL"}, Feedback:{(string.IsNullOrEmpty(it.Feedback) ? "-" : it.Feedback)}");
                sb.AppendLine();
            }

            // ---------- Offers ----------
            var offers = await _db.Offers
                .OrderByDescending(o => o.OfferId)
                .Take(35)
                .Select(o => new { o.OfferId, o.ApplicantId, o.JobOpeningId, o.OfferDate, o.OfferedSalary, o.JoiningDate, o.Status, o.IssuedByUserId })
                .ToListAsync();

            if (offers.Any())
            {
                sb.AppendLine("=== Offers ===");
                foreach (var o in offers)
                    sb.AppendLine($"Id:{o.OfferId}, Applicant:{o.ApplicantId}, Job:{o.JobOpeningId}, OfferDate:{o.OfferDate:yyyy-MM-dd}, Salary:{o.OfferedSalary}, Join:{o.JoiningDate:yyyy-MM-dd}, Status:{o.Status}, IssuedBy:{o.IssuedByUserId ?? "NULL"}");
                sb.AppendLine();
            }

            // ---------- Onboardings ----------
            var onboardings = await _db.Onboardings
                .OrderByDescending(ob => ob.OnboardingId)
                .Take(35)
                .Select(ob => new { ob.OnboardingId, ob.ApplicantId, ob.EmployeeId, ob.StartDate, ob.OrientationCompleted, ob.ResponsibleUserId })
                .ToListAsync();

            if (onboardings.Any())
            {
                sb.AppendLine("=== Onboardings ===");
                foreach (var ob in onboardings)
                    sb.AppendLine($"Id:{ob.OnboardingId}, Applicant:{(ob.ApplicantId == 0 ? "NULL" : ob.ApplicantId.ToString())}, Employee:{ob.EmployeeId}, Start:{ob.StartDate:yyyy-MM-dd}, Orientation:{ob.OrientationCompleted}, ResponsibleUser:{ob.ResponsibleUserId ?? "NULL"}");
                sb.AppendLine();
            }

            // ---------- LeaveTypes ----------
            var leaveTypes = await _db.LeaveTypes
                .OrderBy(lt => lt.LeaveTypeId)
                .Take(35)
                .Select(lt => new { lt.LeaveTypeId, lt.Name, lt.IsPaid, lt.Unit })
                .ToListAsync();

            if (leaveTypes.Any())
            {
                sb.AppendLine("=== LeaveTypes ===");
                foreach (var lt in leaveTypes)
                    sb.AppendLine($"Id:{lt.LeaveTypeId}, Name:{lt.Name}, Paid:{lt.IsPaid}, Unit:{lt.Unit}");
                sb.AppendLine();
            }

            // ---------- LeavePolicies ----------
            var leavePolicies = await _db.LeavePolicies
                .OrderByDescending(lp => lp.LeavePolicyId)
                .Take(35)
                .Select(lp => new { lp.LeavePolicyId, lp.LeaveTypeId, lp.AccrualMethod, lp.EntitlementPerYear })
                .ToListAsync();

            if (leavePolicies.Any())
            {
                sb.AppendLine("=== LeavePolicies ===");
                foreach (var lp in leavePolicies)
                    sb.AppendLine($"Id:{lp.LeavePolicyId}, LeaveType:{lp.LeaveTypeId}, Method:{lp.AccrualMethod}, Entitlement:{lp.EntitlementPerYear}");
                sb.AppendLine();
            }

            // ---------- LeaveRequests ----------
            var leaveRequests = await _db.LeaveRequests
                .OrderByDescending(lr => lr.LeaveRequestId)
                .Take(35)
                .Select(lr => new { lr.LeaveRequestId, lr.EmployeeId, lr.LeaveTypeID, lr.StartDateTime, lr.EndDateTime, lr.DurationHours, lr.Status, lr.RequestedByUserId })
                .ToListAsync();

            if (leaveRequests.Any())
            {
                sb.AppendLine("=== LeaveRequests ===");
                foreach (var lr in leaveRequests)
                    sb.AppendLine($"Id:{lr.LeaveRequestId}, Emp:{lr.EmployeeId}, Type:{lr.LeaveTypeID}, {lr.StartDateTime:yyyy-MM-dd} -> {lr.EndDateTime:yyyy-MM-dd}, Hours:{lr.DurationHours}, Status:{lr.Status}, RequestedBy:{lr.RequestedByUserId ?? "NULL"}");
                sb.AppendLine();
            }

            // ---------- LeaveRequestDays ----------
            var leaveRequestDays = await _db.LeaveRequestDays
                .OrderByDescending(d => d.LeaveRequestDayId)
                .Take(35)
                .Select(d => new { d.LeaveRequestDayId, d.LeaveRequestId, d.TheDate, d.ChargeableHours, d.IsHoliday })
                .ToListAsync();

            if (leaveRequestDays.Any())
            {
                sb.AppendLine("=== LeaveRequestDays ===");
                foreach (var d in leaveRequestDays)
                    sb.AppendLine($"Id:{d.LeaveRequestDayId}, Request:{d.LeaveRequestId}, Date:{d.TheDate:yyyy-MM-dd}, Hours:{d.ChargeableHours}, Holiday:{d.IsHoliday}");
                sb.AppendLine();
            }

            // ---------- LeaveEntitlements ----------
            var entitlements = await _db.LeaveEntitlements
                .OrderByDescending(le => le.LeaveEntitlementId)
                .Take(35)
                .Select(le => new { le.LeaveEntitlementId, le.EmployeeId, le.LeaveTypeId, le.Year, le.Entitled, le.CarriedIn, le.Used })
                .ToListAsync();

            if (entitlements.Any())
            {
                sb.AppendLine("=== LeaveEntitlements ===");
                foreach (var le in entitlements)
                    sb.AppendLine($"Id:{le.LeaveEntitlementId}, Emp:{le.EmployeeId}, Type:{le.LeaveTypeId}, Year:{le.Year}, Entitled:{le.Entitled}, Carried:{le.CarriedIn}, Used:{le.Used}");
                sb.AppendLine();
            }

            // ---------- LeaveLedgers ----------
            var ledgers = await _db.LeaveLedgers
                .OrderByDescending(l => l.LeaveLedgerId)
                .Take(35)
                .Select(l => new { l.LeaveLedgerId, l.EmployeeId, l.LeaveTypeId, l.TxnType, l.Quantity, l.EffectiveDate })
                .ToListAsync();

            if (ledgers.Any())
            {
                sb.AppendLine("=== LeaveLedgers ===");
                foreach (var l in ledgers)
                    sb.AppendLine($"Id:{l.LeaveLedgerId}, Emp:{l.EmployeeId}, Type:{l.LeaveTypeId}, Txn:{l.TxnType}, Qty:{l.Quantity}, Date:{l.EffectiveDate:yyyy-MM-dd}");
                sb.AppendLine();
            }

            // ---------- LeaveApprovals ----------
            var approvals = await _db.LeaveApprovals
                .OrderByDescending(a => a.LeaveApprovalId)
                .Take(35)
                .Select(a => new { a.LeaveApprovalId, a.LeaveRequestId, a.Level, ApproverEmployeeId = a.ApproverEmployeeId, a.ApproverUserId, Action = a.Action, ActionAt = a.ActionAt })
                .ToListAsync();

            if (approvals.Any())
            {
                sb.AppendLine("=== LeaveApprovals ===");
                foreach (var a in approvals)
                    sb.AppendLine($"Id:{a.LeaveApprovalId}, Request:{a.LeaveRequestId}, Level:{a.Level}, ApproverEmp:{(a.ApproverEmployeeId.HasValue ? a.ApproverEmployeeId.Value.ToString() : "NULL")}, ApproverUser:{a.ApproverUserId ?? "NULL"}, Action:{a.Action ?? "NULL"}, At:{(a.ActionAt == default ? "NULL" : a.ActionAt.ToString("yyyy-MM-dd HH:mm"))}");
                sb.AppendLine();
            }

            // ---------- AttendanceRecords ----------
            var attendance = await _db.AttendanceRecords
                .OrderByDescending(a => a.AttendanceRecordId)
                .Take(35)
                .Select(a => new { a.AttendanceRecordId, a.EmployeeId, a.AttendanceDate, a.CheckIn, a.CheckOut, a.WorkHours, a.Status })
                .ToListAsync();

            if (attendance.Any())
            {
                sb.AppendLine("=== AttendanceRecords ===");
                foreach (var a in attendance)
                    sb.AppendLine($"Id:{a.AttendanceRecordId}, Emp:{a.EmployeeId}, Date:{a.AttendanceDate:yyyy-MM-dd}, In:{a.CheckIn:HH:mm}, Out:{a.CheckOut:HH:mm}, Hours:{a.WorkHours}, Status:{a.Status}");
                sb.AppendLine();
            }

            // ---------- OvertimeRequests ----------
            var overtime = await _db.OvertimeRequests
                .OrderByDescending(o => o.OvertimeRequestId)
                .Take(35)
                .Select(o => new { o.OvertimeRequestId, o.EmployeeId, o.OvertimeDate, o.StartTime, o.EndTime, o.Hours, o.Status, o.RequestedByUserId })
                .ToListAsync();

            if (overtime.Any())
            {
                sb.AppendLine("=== OvertimeRequests ===");
                foreach (var o in overtime)
                    sb.AppendLine($"Id:{o.OvertimeRequestId}, Emp:{o.EmployeeId}, Date:{o.OvertimeDate:yyyy-MM-dd}, Start:{o.StartTime:HH:mm}, End:{o.EndTime:HH:mm}, Hours:{o.Hours}, Status:{o.Status}, RequestedBy:{o.RequestedByUserId ?? "NULL"}");
                sb.AppendLine();
            }

            // ---------- Resignations ----------
            var resignations = await _db.Resignations
                .OrderByDescending(r => r.ResignationId)
                .Take(35)
                .Select(r => new { r.ResignationId, r.EmployeeId, r.NoticeDate, r.LastWorkingDate, r.Status, r.RequestedByUserId })
                .ToListAsync();

            if (resignations.Any())
            {
                sb.AppendLine("=== Resignations ===");
                foreach (var r in resignations)
                    sb.AppendLine($"Id:{r.ResignationId}, Emp:{r.EmployeeId}, Notice:{r.NoticeDate:yyyy-MM-dd}, LastWorking:{r.LastWorkingDate:yyyy-MM-dd}, Status:{r.Status}, RequestedBy:{r.RequestedByUserId ?? "NULL"}");
                sb.AppendLine();
            }

            // ---------- ExitClearances ----------
            var exits = await _db.ExitClearances
                .OrderByDescending(x => x.ExitClearanceId)
                .Take(35)
                .Select(x => new { x.ExitClearanceId, x.ResignationId, x.HRCleared, x.ITCleared, x.FinanceCleared, x.FinalSettlementAmt, x.HRClearedByUserId, x.ITClearedByUserId, x.FinanceClearedByUserId })
                .ToListAsync();

            if (exits.Any())
            {
                sb.AppendLine("=== ExitClearances ===");
                foreach (var x in exits)
                    sb.AppendLine($"Id:{x.ExitClearanceId}, Resignation:{x.ResignationId}, HR:{x.HRCleared}, IT:{x.ITCleared}, Finance:{x.FinanceCleared}, Settlement:{x.FinalSettlementAmt}, HRBy:{x.HRClearedByUserId ?? "NULL"}, ITBy:{x.ITClearedByUserId ?? "NULL"}, FinBy:{x.FinanceClearedByUserId ?? "NULL"}");
                sb.AppendLine();
            }

            // ---------- PayrollRuns ----------
            var payrollRuns = await _db.PayrollRuns
                .OrderByDescending(pr => pr.PayrollRunId)
                .Take(35)
                .Select(pr => new { pr.PayrollRunId, pr.PeriodStart, pr.PeriodEnd, pr.RunDate, pr.RunByUserId })
                .ToListAsync();

            if (payrollRuns.Any())
            {
                sb.AppendLine("=== PayrollRuns ===");
                foreach (var pr in payrollRuns)
                    sb.AppendLine($"Id:{pr.PayrollRunId}, Period:{pr.PeriodStart:yyyy-MM-dd} -> {pr.PeriodEnd:yyyy-MM-dd}, RunDate:{pr.RunDate:yyyy-MM-dd}, RunBy:{pr.RunByUserId ?? "NULL"}");
                sb.AppendLine();
            }

            // ---------- Payrolls ----------
            var payrolls = await _db.Payrolls
                .OrderByDescending(p => p.PayrollId)
                .Take(35)
                .Select(p => new { p.PayrollId, p.PayrollRunId, p.EmployeeId, p.BaseSalary, p.GrossEarnings, p.NetSalary, p.PaymentDate })
                .ToListAsync();

            if (payrolls.Any())
            {
                sb.AppendLine("=== Payrolls ===");
                foreach (var p in payrolls)
                    sb.AppendLine($"Id:{p.PayrollId}, Run:{p.PayrollRunId}, Emp:{p.EmployeeId}, Base:{p.BaseSalary}, Gross:{p.GrossEarnings}, Net:{p.NetSalary}, Paid:{p.PaymentDate:yyyy-MM-dd}");
                sb.AppendLine();
            }

            // ---------- PayrollDeductions ----------
            var payrollDeds = await _db.PayrollDeductions
                .OrderByDescending(pd => pd.PayrollDeductionId)
                .Take(35)
                .Select(pd => new { pd.PayrollDeductionId, pd.PayrollId, pd.DeductionId, pd.Amount })
                .ToListAsync();

            if (payrollDeds.Any())
            {
                sb.AppendLine("=== PayrollDeductions ===");
                foreach (var pd in payrollDeds)
                    sb.AppendLine($"Id:{pd.PayrollDeductionId}, Payroll:{pd.PayrollId}, Deduction:{pd.DeductionId}, Amount:{pd.Amount}");
                sb.AppendLine();
            }

            // ---------- Deductions ----------
            var deductions = await _db.Deductions
                .OrderBy(d => d.DeductionId)
                .Take(35)
                .Select(d => new { d.DeductionId, d.Name, d.IsPercentage, d.AmountOrPercent })
                .ToListAsync();

            if (deductions.Any())
            {
                sb.AppendLine("=== Deductions ===");
                foreach (var d in deductions)
                    sb.AppendLine($"Id:{d.DeductionId}, Name:{d.Name}, IsPct:{d.IsPercentage}, Amount:{d.AmountOrPercent}");
                sb.AppendLine();
            }

            // ---------- HolidayCalendars & Holidays ----------
            var calendars = await _db.HolidayCalendars
                .OrderBy(hc => hc.HolidayCalendarId)
                .Take(35)
                .Select(hc => new { hc.HolidayCalendarId, hc.Name })
                .ToListAsync();

            if (calendars.Any())
            {
                sb.AppendLine("=== HolidayCalendars ===");
                foreach (var hc in calendars)
                    sb.AppendLine($"Id:{hc.HolidayCalendarId}, Name:{hc.Name}");
                sb.AppendLine();
            }

            var holidays = await _db.Holidays
                .OrderByDescending(h => h.HolidayId)
                .Take(35)
                .Select(h => new { h.HolidayId, h.HolidayCalendarId, h.HolidayDate, h.Name })
                .ToListAsync();

            if (holidays.Any())
            {
                sb.AppendLine("=== Holidays ===");
                foreach (var h in holidays)
                    sb.AppendLine($"Id:{h.HolidayId}, Calendar:{h.HolidayCalendarId}, Date:{h.HolidayDate:yyyy-MM-dd}, Name:{h.Name}");
                sb.AppendLine();
            }

            // ---------- Finally: small fallback message ----------
            return sb.Length > 0 ? sb.ToString() : "No records available in database.";
        }

    }
}
