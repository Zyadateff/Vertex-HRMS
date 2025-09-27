using VertexHRMS.BLL.Services.Abstraction;

namespace VertexHRMS.BLL.Services.Implementation
{
    public class ConsoleEmailService : IEmailService
    {
        public Task SendAsync(string to, string subject, string htmlBody)
        {
            Console.WriteLine($"[EMAIL] To:{to} | Subject:{subject}");
            return Task.CompletedTask;
        }
    }
}
