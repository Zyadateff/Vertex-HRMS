namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IDashboardService
    {
        Task<DashboardVM> GetDashboardAsync(DateTime? from = null, DateTime? to = null, string department = null);
    }
}
