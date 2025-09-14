using VertexHRMS.BLL.ModelVM.AI;
namespace VertexHRMS.BLL.Services.Abstraction
{
    public interface IAIService
    {
        Task<(bool, string?, AIVM)> GetAnswerAsync(string prompt);
    }
}
