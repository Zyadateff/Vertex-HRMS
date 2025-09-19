
using MailKit;
using MailKit.Net.Imap;
using MimeKit;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class EmailService:IEmailService
    {
        private readonly string _email = "vertexhrms32@gmail.com";
        private readonly string _password = "zpiltowjdszgrqcg"; 
        private readonly string _imapServer = "imap.gmail.com"; 
        private readonly int _imapPort = 993;
        private readonly ILeaveRequestRepo _leaveRequestRepo;
        private readonly EmailSenderService _emailSender;
        public EmailService(ILeaveRequestRepo leaveRequestRepo, EmailSenderService emailSender)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _emailSender = emailSender;
        }
        public async Task CheckInboxAsync()
        {
            Console.WriteLine("Job Started");
            try
            {
                using var client = new ImapClient();
                await client.ConnectAsync(_imapServer, _imapPort, true);
                await client.AuthenticateAsync(_email, _password);
                var inbox = client.Inbox;
                await inbox.OpenAsync(FolderAccess.ReadWrite);
                var uids = await inbox.SearchAsync(MailKit.Search.SearchQuery.NotSeen);
                foreach (var uid in uids)
                {
                    try
                    {
                        var message = await inbox.GetMessageAsync(uid);
                        string from = message.From.Mailboxes.First().Address;
                        if (!message.Subject.Contains("Leave Request")) continue;
                        var body = message.TextBody;
                        var lines = body.Split('\n');
                        int employeeId = int.Parse(lines[0].Replace("EmployeeId:", "").Trim());
                        int leaveTypeId = int.Parse(lines[1].Replace("LeaveTypeID:", "").Trim());
                        DateTime start = DateTime.Parse(lines[2].Replace("Start:", "").Trim());
                        DateTime end = DateTime.Parse(lines[3].Replace("End:", "").Trim());
                        string reason = lines[4].Replace("Reason:", "").Trim();
                        await _emailSender.SendEmailAsync(from, ".", "حمرا");
                        await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                    }
                    catch(Exception ex)
                    {
                        Console.WriteLine($"❌ Error processing email: {ex.Message}");
                    }
                }
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error: {ex.Message}");
            }
        }
    }
}
