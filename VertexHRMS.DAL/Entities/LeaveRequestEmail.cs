using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.DAL.Entities
{
    public class LeaveRequestEmail
    {
        public LeaveRequestEmail( int employeeId, string fromEmail, string subject, string body, DateTime receivedAt, bool hasAttachment, int leaveRequestId, string replySubject, string replyBody)
        {
            EmployeeId = employeeId;
            FromEmail = fromEmail;
            Subject = subject;
            Body = body;
            ReceivedAt = receivedAt;
            HasAttachment = hasAttachment;
            LeaveRequestId = leaveRequestId;
            ReplySubject = replySubject;
            ReplyBody = replyBody;
        }

        public int LeaveRequestEmailId { get; private set; }
        public int EmployeeId { get; private set; }
        public string FromEmail { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public DateTime ReceivedAt { get; private set; }
        public bool HasAttachment { get; private set; }
        public int LeaveRequestId { get; private set; }
        public LeaveRequest LeaveRequest { get; private set; }
        public string ReplySubject { get; private set; }
        public string ReplyBody { get; private set; }
    }
}
