
namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string to, string subject, string body);
    }
}
