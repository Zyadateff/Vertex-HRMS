using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VertexHRMS.BLL.ModelVM.LeaveRequestVM
{
    public class GetPendingRequestsVM
    {
        public int LeaveRequestId { get;  set; }
        public int EmployeeId { get;  set; }
        public Employee Employee { get; set; }
        public int LeaveTypeID { get; set; }
        public LeaveType LeaveType { get; set; }
        public DateTime StartDateTime { get; set; }
        public DateTime EndDateTime { get; set; }
        public decimal DurationHours { get; set; }
        public string RequestedByUserId { get; set; }
        public ApplicationUser RequestedByUser { get;  set; }
    }
}
