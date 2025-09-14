namespace VertexHRMS.DAL.Repo.Abstraction
{
    public interface IAIRepo
    {
        Task<string> AskAsync(string prompt);
    }
}
