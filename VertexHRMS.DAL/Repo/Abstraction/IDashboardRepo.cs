namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IDashboardRepo
    {
        // Simple aggregates
        Task<int> CountEmployeesAsync(string department = null);
        Task<int> CountActivePositionsAsync();
        Task<int> CountOpenJobReqsAsync();

        // Payroll
        Task<decimal> GetPayrollSumAsync(DateTime fromInclusive, DateTime toExclusive, string department = null);
        Task<Dictionary<DateTime, decimal>> GetPayrollSeriesAsync(DateTime fromMonthStart, DateTime toMonthStartExclusive, string department = null);

        // Headcount & departments
        Task<Dictionary<DateTime, int>> GetHeadcountSeriesAsync(DateTime fromMonthStart, DateTime toMonthStartInclusive, string department = null);
        Task<Dictionary<string, int>> GetDeptDistributionAsync(string departmentFilter = null);
        Task<List<(string Name, string Dept, DateTime Hired)>> GetRecentHiresAsync(int limit = 5, string department = null);

        // Recruitment
        Task<int> CountApplicantsAsync(DateTime? from = null, DateTime? to = null);
        Task<int> CountInterviewsAsync(DateTime? from = null, DateTime? to = null);
        Task<int> CountOffersAsync(DateTime? from = null, DateTime? to = null);

        // Revenue & Projects
        Task<Dictionary<DateTime, decimal>> GetRevenueSeriesAsync(DateTime fromMonthStart, DateTime toMonthStartExclusive);
        Task<List<(string ProjectName, decimal Budget)>> GetTopProjectsByBudgetAsync(int top = 5);
        Task<Dictionary<string, decimal>> GetProjectUtilizationAsync(int? projectId = null);

        // Trainings & attendance
        Task<(int Total, int Completed, int InProgress)> GetTrainingsSummaryAsync();
        Task<(int Count, decimal AvgMinutes)> GetAttendanceSummaryAsync(DateTime fromInclusive, DateTime toExclusive);
    }
}
