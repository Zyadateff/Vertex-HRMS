using MailKit.Net.Imap;
using MailKit;
using MimeKit;
using System;
using System.Linq;
using System.Threading.Tasks;
using VertexHRMS.BLL.ModelVM.LeaveRequestVM;
using VertexHRMS.DAL.Entities;
using VertexHRMS.DAL.Repo.Abstraction;

namespace VertexHRMS.BLL.Service.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly string _email = "vertexhrms@gmail.com";
        private readonly string _password = "szluvvxqsywatvst";
        private readonly string _imapServer = "imap.gmail.com";
        private readonly int _imapPort = 993;
        private readonly ILeaveRequestRepo _leaveRequestRepo;
        private readonly ILeaveRequestService _leaveRequestService;
        private readonly IEmailSenderService _emailSender;
        private readonly IEmployeeRepo _employeeRepo;
        private readonly ILeaveRequestEmailRepo _leaveRequestEmailRepo;

        public EmailService(
            ILeaveRequestRepo leaveRequestRepo,
            IEmailSenderService emailSender,
            ILeaveRequestService leaveRequestService,
            IEmployeeRepo employeeRepo,
            ILeaveRequestEmailRepo leaveRequestEmailRepo)
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
                        string s = hasPdf ? "YES" : null;
                        string from = message.From.Mailboxes.First().Address;
                        if (!message.Subject.Contains("Leave Request")) { await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true); continue; }

                        var body = message.TextBody ?? message.HtmlBody ?? string.Empty;
                        var lines = body.Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                        if (lines.Length < 4)
                        {
                            Console.WriteLine("Email format invalid - skipping");
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        if (!int.TryParse(lines[0].Replace("EmployeeId:", "").Trim(), out var employeeId))
                        {
                            Console.WriteLine("Invalid EmployeeId in email - skipping");
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        if (!int.TryParse(lines[1].Replace("LeaveTypeID:", "").Trim(), out var leaveTypeId))
                        {
                            Console.WriteLine("Invalid LeaveTypeID in email - skipping");
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        if (!DateTime.TryParse(lines[2].Replace("Start:", "").Trim(), out var start))
                        {
                            Console.WriteLine("Invalid Start date - skipping");
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        if (!DateTime.TryParse(lines[3].Replace("End:", "").Trim(), out var end))
                        {
                            Console.WriteLine("Invalid End date - skipping");
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        string reason = lines.Length > 4 ? lines[4].Replace("Reason:", "").Trim() : "";

                        var request = new AddVM(employeeId, leaveTypeId, start, end);
                        await _leaveRequestService.SubmitLeaveRequestAsync(request, s);

                        // الحصول على آخر Request للموظف (الذي تم إنشاؤه)
                        var req = await _leaveRequestRepo.GetLatestByEmployeeAsync(employeeId);
                        if (req == null)
                        {
                            Console.WriteLine("No request found after submit.");
                            await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                            continue;
                        }

                        var emp = await _employeeRepo.GetByIdAsync(employeeId);
                        string reply =
                            $@"Hello {emp?.FirstName ?? ""},<br/><br/>
                            Your leave request from {request.StartDateTime:dd/MM/yyyy} to {request.EndDateTime:dd/MM/yyyy}<br/>
                            has been <span style='color:{(req.Status == "Approved" ? "#0f9d58" : req.Status == "Rejected" ? "#d32f2f" : "#a67c00")};
                                         font-weight:bold;padding:4px 8px;border-radius:4px;'>{System.Net.WebUtility.HtmlEncode(req.Status)}</span>.<br/><br/>
                            Reason: {req.RejectionReason ?? "None"}";
                        string reply2= $@"
                          <p>Hello {System.Net.WebUtility.HtmlEncode(emp?.FirstName ?? "")},</p>
                          <p>
                            Your leave request from <strong>{request.StartDateTime:dd/MM/yyyy}</strong>
                            to <strong>{request.EndDateTime:dd/MM/yyyy}</strong><br>
has been
                            <span style='color:{(req.Status == "Approved" ? "#0f9d58" : req.Status == "Rejected" ? "#d32f2f" : "#a67c00")};
                                         font-weight:bold;padding:4px 8px;border-radius:4px;'>{System.Net.WebUtility.HtmlEncode(req.Status)}</span>.
                          </p>
                          <p><strong>Reason:</strong> {System.Net.WebUtility.HtmlEncode(req.RejectionReason ?? "None")}</p>
                          <hr/>
                          <p style='font-size:12px;color:#666;'>If you have questions contact HR.</p>
                        ";

                        var emailLog = new LeaveRequestEmail(
                            employeeId,
                            from,
                            message.Subject,
                            body,
                            DateTime.Now,
                            hasPdf,
                            req.LeaveRequestId,
                            "Reply to your leave request",
                            reply);

                        await _leaveRequestEmailRepo.AddAsync(emailLog);
                        await _emailSender.SendEmailAsync(from, "Reply to your leave request", reply2);
                        await inbox.AddFlagsAsync(uid, MessageFlags.Seen, true);
                    }
                    catch (Exception ex)
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
