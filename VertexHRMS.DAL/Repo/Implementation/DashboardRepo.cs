namespace VertexHRMS.DAL.Repo.Implementation
{
    public class DashboardRepo : IDashboardRepo
    {
        private readonly VertexHRMSDbContext _db;
        public DashboardRepo(VertexHRMSDbContext db)
        {
            _db = db;
        }

        public Task<int> CountEmployeesAsync(string department = null)
        {
            var q = _db.Employees.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(department))
                q = q.Where(e => e.Department != null && e.Department.DepartmentName == department);
            return q.CountAsync();
        }

        public Task<int> CountActivePositionsAsync()
        {
            if (_db.Positions != null) return _db.Positions.AsNoTracking().CountAsync();
            return _db.Employees.AsNoTracking().Select(e => e.PositionID).Distinct().CountAsync();
        }

        public Task<int> CountOpenJobReqsAsync()
        {
            return _db.JobOpenings.AsNoTracking().CountAsync(j => j.Status != null && j.Status.ToLower() == "open");
        }

        public Task<decimal> GetPayrollSumAsync(DateTime fromInclusive, DateTime toExclusive, string department = null)
        {
            var q = _db.Payrolls.AsNoTracking().Where(p => p.PaymentDate >= fromInclusive && p.PaymentDate < toExclusive);
            if (!string.IsNullOrWhiteSpace(department))
                q = q.Where(p => p.Employee != null && p.Employee.Department != null && p.Employee.Department.DepartmentName == department);

            return q.Select(p => (decimal?)p.NetSalary).SumAsync().ContinueWith(t => t.Result ?? 0m);
        }

        public async Task<Dictionary<DateTime, decimal>> GetPayrollSeriesAsync(DateTime fromMonthStart, DateTime toMonthStartExclusive, string department = null)
        {
            var q = _db.Payrolls.AsNoTracking().Where(p => p.PaymentDate >= fromMonthStart && p.PaymentDate < toMonthStartExclusive);
            if (!string.IsNullOrWhiteSpace(department))
                q = q.Where(p => p.Employee != null && p.Employee.Department != null && p.Employee.Department.DepartmentName == department);

            var groups = await q.GroupBy(p => new { p.PaymentDate.Year, p.PaymentDate.Month })
                                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Sum = g.Sum(x => x.NetSalary) })
                                .ToListAsync();

            return groups.ToDictionary(g => new DateTime(g.Year, g.Month, 1, 0, 0, 0, DateTimeKind.Utc), g => g.Sum);
        }

        public async Task<Dictionary<DateTime, int>> GetHeadcountSeriesAsync(DateTime fromMonthStart, DateTime toMonthStartInclusive, string department = null)
        {
            var toMonthEnd = toMonthStartInclusive.AddMonths(1).AddTicks(-1);
            var q = _db.Employees.AsNoTracking().Where(e => e.HireDate <= toMonthEnd);
            if (!string.IsNullOrWhiteSpace(department))
                q = q.Where(e => e.Department != null && e.Department.DepartmentName == department);

            var hires = await q.Select(e => e.HireDate).ToListAsync();

            var grouped = hires.GroupBy(d => new { d.Year, d.Month })
                               .Select(g => new { MonthStart = new DateTime(g.Key.Year, g.Key.Month, 1, 0, 0, 0, DateTimeKind.Utc), Count = g.Count() })
                               .ToDictionary(x => x.MonthStart, x => x.Count);

            var monthCount = ((toMonthStartInclusive.Year - fromMonthStart.Year) * 12) + (toMonthStartInclusive.Month - fromMonthStart.Month) + 1;
            if (monthCount <= 0) monthCount = 1;
            var months = Enumerable.Range(0, monthCount).Select(i => fromMonthStart.AddMonths(i)).ToList();

            var result = new Dictionary<DateTime, int>();
            var running = hires.Count(d => d < fromMonthStart);
            foreach (var m in months)
            {
                if (grouped.TryGetValue(m, out var c)) running += c;
                result[m] = running;
            }
            return result;
        }

        public async Task<Dictionary<string, int>> GetDeptDistributionAsync(string departmentFilter = null)
        {
            var q = _db.Employees.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(departmentFilter))
                q = q.Where(e => e.Department != null && e.Department.DepartmentName == departmentFilter);

            var groups = await q.GroupBy(e => e.Department.DepartmentName)
                                .Select(g => new { Dept = g.Key, Count = g.Count() })
                                .OrderByDescending(x => x.Count)
                                .ToListAsync();

            return groups.ToDictionary(x => x.Dept ?? "Unknown", x => x.Count);
        }

        public async Task<List<(string Name, string Dept, DateTime Hired)>> GetRecentHiresAsync(int limit = 5, string department = null)
        {
            var q = _db.Employees.AsNoTracking().AsQueryable();
            if (!string.IsNullOrWhiteSpace(department))
                q = q.Where(e => e.Department != null && e.Department.DepartmentName == department);

            var list = await q.OrderByDescending(e => e.HireDate)
                              .Take(limit)
                              .Select(e => new { Name = (e.FirstName ?? "") + " " + (e.LastName ?? ""), Dept = e.Department.DepartmentName, e.HireDate })
                              .ToListAsync();

            return list.Select(x => (x.Name, x.Dept ?? "Unknown", x.HireDate)).ToList();
        }

        public Task<int> CountApplicantsAsync(DateTime? from = null, DateTime? to = null)
        {
            var q = _db.Applicants.AsNoTracking().AsQueryable();
            if (from.HasValue) q = q.Where(a => a.AppliedDate >= from.Value);
            if (to.HasValue) q = q.Where(a => a.AppliedDate < to.Value);
            return q.CountAsync();
        }

        public Task<int> CountInterviewsAsync(DateTime? from = null, DateTime? to = null)
        {
            var q = _db.Interviews.AsNoTracking().AsQueryable();
            if (from.HasValue) q = q.Where(i => i.InterviewDate >= from.Value);
            if (to.HasValue) q = q.Where(i => i.InterviewDate < to.Value);
            return q.CountAsync();
        }

        public Task<int> CountOffersAsync(DateTime? from = null, DateTime? to = null)
        {
            var q = _db.Offers.AsNoTracking().AsQueryable();
            if (from.HasValue) q = q.Where(o => o.OfferDate >= from.Value);
            if (to.HasValue) q = q.Where(o => o.OfferDate < to.Value);
            return q.CountAsync();
        }

        public async Task<Dictionary<DateTime, decimal>> GetRevenueSeriesAsync(DateTime fromMonthStart, DateTime toMonthStartExclusive)
        {
            var q = _db.Revenues.AsNoTracking().Where(r => r.MonthYear >= fromMonthStart && r.MonthYear < toMonthStartExclusive);

            var groups = await q.GroupBy(r => new { r.MonthYear.Year, r.MonthYear.Month })
                                .Select(g => new { Year = g.Key.Year, Month = g.Key.Month, Sum = g.Sum(x => x.Amount) })
                                .ToListAsync();

            return groups.ToDictionary(g => new DateTime(g.Year, g.Month, 1, 0, 0, 0, DateTimeKind.Utc), g => g.Sum);
        }

        public async Task<List<(string ProjectName, decimal Budget)>> GetTopProjectsByBudgetAsync(int top = 5)
        {
            var list = await _db.Projects.AsNoTracking()
                        .OrderByDescending(p => p.Budget)
                        .Take(top)
                        .Select(p => new { p.Name, p.Budget })
                        .ToListAsync();

            return list.Select(x => (x.Name, x.Budget)).ToList();
        }

        public async Task<Dictionary<string, decimal>> GetProjectUtilizationAsync(int? projectId = null)
        {
            var q = _db.ProjectTasks.AsNoTracking().AsQueryable();
            if (projectId.HasValue) q = q.Where(t => t.ProjectId == projectId.Value);

            var groups = await q.GroupBy(t => t.ProjectId)
                                .Select(g => new {
                                    ProjectId = g.Key,
                                    Estimated = g.Sum(x => (int?)x.EstimatedHours) ?? 0,
                                    Spent = g.Sum(x => (int?)x.SpentHours) ?? 0
                                })
                                .ToListAsync();

            var projIds = groups.Select(g => g.ProjectId).Where(id => id != null).ToList();
            var projects = await _db.Projects.AsNoTracking().Where(p => projIds.Contains(p.Id)).ToListAsync();

            var map = new Dictionary<string, decimal>();
            foreach (var g in groups)
            {
                var proj = projects.FirstOrDefault(p => p.Id == g.ProjectId);
                var util = g.Estimated == 0 ? 0m : Math.Round((decimal)g.Spent / g.Estimated * 100, 2);
                map[proj?.Name ?? $"Project {g.ProjectId}"] = util;
            }
            return map;
        }

        public async Task<(int Total, int Completed, int InProgress)> GetTrainingsSummaryAsync()
        {
            var q = _db.EmployeeTrainings.AsNoTracking();
            var total = await q.CountAsync();
            var completed = await q.CountAsync(t => t.Status == "Completed");
            var inProgress = await q.CountAsync(t => t.Status == "InProgress");
            return (total, completed, inProgress);
        }

        public async Task<(int Count, decimal AvgMinutes)> GetAttendanceSummaryAsync(DateTime fromInclusive, DateTime toExclusive)
        {
            var q = _db.AttendanceRecords.AsNoTracking().Where(a => a.AttendanceDate >= fromInclusive && a.AttendanceDate < toExclusive && a.WorkHours.HasValue);
            var list = await q.Select(a => a.WorkHours.Value).ToListAsync();
            var count = list.Count;
            var avg = count == 0 ? 0m : Math.Round((decimal)list.Average(), 2);
            return (count, avg);
        }
    }
}
