using VertexHRMS.BLL.ModelVM.AI;
using VertexHRMS.BLL.Services.Abstraction;
using VertexHRMS.DAL.Repo.Abstraction;
namespace VertexHRMS.BLL.Services.Implementation
{
    public class AIService : IAIService
    {
        private readonly IAIRepo AIrepo;
        public AIService(IAIRepo AIrepo)
        {
            this.AIrepo = AIrepo;
        }
        public async Task<(bool, string?, AIVM)> GetAnswerAsync(string prompt)
        {
            var answer = await AIrepo.AskAsync(prompt);
            if (string.IsNullOrEmpty(answer))
            {
                return (false, "No answer found", new AIVM
                {
                    Question = prompt,
                    Answer = "I'm sorry, I couldn't find an answer to your question."
                });
            }
            return (true, null, new AIVM { Question = prompt, Answer = answer });
        }
    }
}
