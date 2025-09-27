namespace VertexHRMS.BLL.Services.Abstraction
{
    public interface IFormIngestService
    {
        /// <summary>Pulls new responses from the external "Google Form responses" table into GoogleFormApplications.</summary>
        Task ImportNewAsync(CancellationToken ct = default);
    }
}
