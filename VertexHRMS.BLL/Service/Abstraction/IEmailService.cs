
namespace VertexHRMS.BLL.Service.Abstraction
{
    public interface IEmailService
    {
        Task CheckInboxAsync();

namespace VertexHRMS.BLL.Services.Abstraction
{
    public interface IEmailService
    {
        Task SendAsync(string to, string subject, string htmlBody);
    }
}
