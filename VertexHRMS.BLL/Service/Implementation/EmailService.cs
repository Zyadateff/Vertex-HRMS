
using MailKit;
using MailKit.Net.Imap;
using MimeKit;
using VertexHRMS.BLL.ModelVM.LeaveRequestVM;
using VertexHRMS.DAL.Entities;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class EmailService:IEmailService
    {
        private readonly string _email = "vertexhrms32@gmail.com";
        private readonly string _password = "zpiltowjdszgrqcg"; 
        private readonly string _imapServer = "imap.gmail.com"; 
        private readonly int _imapPort = 993;
        private readonly ILeaveRequestRepo _leaveRequestRepo;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IEmailSenderService _emailSender;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ILeaveRequestEmailRepo _leaveRequestEmailRepo;
        public EmailService(ILeaveRequestRepo leaveRequestRepo, IEmailSenderService emailSender, ILeaveRequestService leaveRequestService, IEmployeeRepo employeeRepo, ILeaveRequestEmailRepo leaveRequestEmailRepo)
        {
            _leaveRequestRepo = leaveRequestRepo;
            _emailSender = emailSender;
            _leaveRequestService = leaveRequestService;
            _employeeRepo = employeeRepo;
            _leaveRequestEmailRepo = leaveRequestEmailRepo;
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
                        bool hasPdf = message.Attachments.OfType<MimePart>().Any(att => att.FileName != null && att.FileName.EndsWith(".pdf", StringComparison.OrdinalIgnoreCase));
                        string s = null;
                        if (hasPdf)
                        {
                            s = "YES";
                        }
                        string from = message.From.Mailboxes.First().Address;
                        if (!message.Subject.Contains("Leave Request")) continue;
                        var body = message.TextBody;
                        var lines = body.Split('\n');
                        int employeeId = int.Parse(lines[0].Replace("EmployeeId:", "").Trim());
                        int leaveTypeId = int.Parse(lines[1].Replace("LeaveTypeID:", "").Trim());
                        DateTime start = DateTime.Parse(lines[2].Replace("Start:", "").Trim());
                        DateTime end = DateTime.Parse(lines[3].Replace("End:", "").Trim());
                        string reason = lines[4].Replace("Reason:", "").Trim();
                        var request =new AddVM(employeeId, leaveTypeId, start, end);
                        await _leaveRequestService.SubmitLeaveRequestAsync(request, s);
                        var req = await _leaveRequestService.GetRequestByIdAsync (employeeId);
                        var emp = await _employeeRepo .GetByIdAsync (employeeId);   
                        string reply = $@"
                                         Hello,

                                         Your leave request from {request.StartDateTime:dd/MM/yyyy} to {request.EndDateTime:dd/MM/yyyy} 
                                         has been <span style='color:red; font-weight:bold;'>{req.Status}</span>.</p>.

                                         Reason:{req.RejectionReason?? "None"}
                                         ";
                        var emailLog = new LeaveRequestEmail
                        (
                            employeeId,
                            from,
                            message.Subject,
                            body,
                            DateTime.Now,
                            hasPdf,
                            req.LeaveRequestId,
                            "Reply to your leave request",
                            reply
                        );
                        await _leaveRequestEmailRepo.AddAsync ( emailLog );
                        await _emailSender.SendEmailAsync(from, "Reply to your leave request", reply);
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
