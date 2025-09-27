using MailKit.Net.Smtp;
using MimeKit;


namespace VertexHRMS.BLL.Service.Implementation
{
    public class EmailSenderService:IEmailSenderService
    {
        private readonly string _email = "vertexhrms32@gmail.com";
        private readonly string _password = "zpiltowjdszgrqcg";

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("HR System", _email));
            message.To.Add(new MailboxAddress("", to));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            using var client = new SmtpClient();
            await client.ConnectAsync("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
            await client.AuthenticateAsync(_email, _password);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
