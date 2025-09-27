namespace VertexHRMS.BLL.Service.Implementation
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepo _repo;
        public DashboardService(IDashboardRepo repo)
        {
            _repo = repo;
        }

        public async Task<DashboardVM> GetDashboardAsync(DateTime? from = null, DateTime? to = null, string department = null)
        {
            var toDt = (to ?? DateTime.UtcNow).ToUniversalTime();
            var fromDt = (from ?? toDt.AddMonths(-5)).ToUniversalTime();

            var fromMonthStart = new DateTime(fromDt.Year, fromDt.Month, 1, 0, 0, 0, DateTimeKind.Utc);
            var toMonthStart = new DateTime(toDt.Year, toDt.Month, 1, 0, 0, 0, DateTimeKind.Utc);

            // sequential queries (avoid DbContext concurrency)
            var empCount = await _repo.CountEmployeesAsync(department);
            var activePositions = await _repo.CountActivePositionsAsync();
            var openJobReqs = await _repo.CountOpenJobReqsAsync();

            var payrollThisMonth = await _repo.GetPayrollSumAsync(toMonthStart, toMonthStart.AddMonths(1), department);
            var payrollPrevMonth = await _repo.GetPayrollSumAsync(toMonthStart.AddMonths(-1), toMonthStart, department);

            var payrollSeries = await _repo.GetPayrollSeriesAsync(fromMonthStart, toMonthStart.AddMonths(1), department);
            var headcountSeries = await _repo.GetHeadcountSeriesAsync(fromMonthStart, toMonthStart, department);
            var deptDist = await _repo.GetDeptDistributionAsync(department);
            var recentHires = await _repo.GetRecentHiresAsync(6, department);

            var applicants = await _repo.CountApplicantsAsync(fromMonthStart, toMonthStart.AddMonths(1));
            var interviews = await _repo.CountInterviewsAsync(fromMonthStart, toMonthStart.AddMonths(1));
            var offers = await _repo.CountOffersAsync(fromMonthStart, toMonthStart.AddMonths(1));

            var revenueSeries = await _repo.GetRevenueSeriesAsync(fromMonthStart, toMonthStart.AddMonths(1));
            var topProjects = await _repo.GetTopProjectsByBudgetAsync(10);
            var projectUtil = await _repo.GetProjectUtilizationAsync(null);

            var trainings = await _repo.GetTrainingsSummaryAsync();
            var attendance = await _repo.GetAttendanceSummaryAsync(fromMonthStart, toMonthStart.AddMonths(1));

            var vm = new DashboardVM
            {
                Employees = empCount,
                ActivePositions = activePositions,
                OpenJobReqs = openJobReqs,
                PayrollThisMonth = payrollThisMonth,
                PayrollDeltaPercent = payrollPrevMonth == 0 ? 0m : Math.Round((payrollThisMonth - payrollPrevMonth) / payrollPrevMonth * 100, 2)
            };

            // payroll series -> labels/values sorted by date
            var payrollKeys = payrollSeries.Keys.OrderBy(k => k).ToList();
            vm.PayrollLabels = payrollKeys.Select(d => d.ToString("MMM yyyy")).ToList();
            vm.PayrollValues = payrollKeys.Select(d => payrollSeries[d]).ToList();

            // headcount series
            var headKeys = headcountSeries.Keys.OrderBy(k => k).ToList();
            vm.HeadcountLabels = headKeys.Select(d => d.ToString("MMM yyyy")).ToList();
            vm.HeadcountValues = headKeys.Select(d => headcountSeries[d]).ToList();

            // dept distribution
            vm.DeptNames = deptDist.Keys.ToList();
            vm.DeptValues = deptDist.Values.ToList();

            // recent hires
            vm.RecentHireNames = recentHires.Select(r => r.Name).ToList();
            vm.RecentHireDepts = recentHires.Select(r => r.Dept).ToList();
            vm.RecentHireDates = recentHires.Select(r => r.Hired).ToList();

            vm.Applicants = applicants;
            vm.Interviews = interviews;
            vm.Offers = offers;

            // revenue
            var revKeys = revenueSeries.Keys.OrderBy(k => k).ToList();
            vm.RevenueLabels = revKeys.Select(d => d.ToString("MMM yyyy")).ToList();
            vm.RevenueValues = revKeys.Select(d => revenueSeries[d]).ToList();

            // top projects
            vm.TopProjectNames = topProjects.Select(p => p.ProjectName).ToList();
            vm.TopProjectBudgets = topProjects.Select(p => p.Budget).ToList();

            // project util
            vm.ProjectUtilNames = projectUtil.Keys.ToList();
            vm.ProjectUtilPercent = projectUtil.Values.ToList();

            // trainings & attendance
            vm.TrainingsTotal = trainings.Total;
            vm.TrainingsCompleted = trainings.Completed;
            vm.TrainingsInProgress = trainings.InProgress;

            vm.AttendanceRecordsThisMonth = attendance.Count;
            vm.AverageWorkMinutes = attendance.AvgMinutes;

            vm.DataNote = $"Applicants: {applicants}, Interviews: {interviews}, Offers: {offers}";

            if (!vm.RevenueValues.Any() && vm.HeadcountValues.All(v => v <= 1) && vm.RecentHireNames.Count == 0)
            {
                vm.IsDataLimited = true;
                vm.DataNote = "Limited data / test data only";
            }

            return vm;
        }
    }
}
