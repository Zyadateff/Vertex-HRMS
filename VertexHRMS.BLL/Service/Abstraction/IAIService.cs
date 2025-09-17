namespace VertexHRMS.BLL.Services.Abstraction
{
    public interface IAIService
    {
        Task<AIVM> GetAnswerAsync(string question);
    }
}
