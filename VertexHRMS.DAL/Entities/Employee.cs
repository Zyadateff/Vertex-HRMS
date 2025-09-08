
namespace VertexHRMS.DAL.Entities
{
    public class Employee
    {
        public Employee()
        {

        }
        public int EmployeeId { get; private set; }
        public string ImagePath { get; private set; }
        public string EmployeeCode { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public DateTime HireDate { get; private set; }
        public string EmploymentType { get; private set; }
        public string Status { get; private set; }
        public int DepartmentId { get; private set; }
        public Department Department { get; private set; }
        public int PositionID { get; private set; }
        public Position Position { get; private set; }
        public int? ManagerId { get; private set; }
        public Employee Manager { get; private set; }
        public ICollection<Employee> DirectReports { get; private set; } = new List<Employee>();
        public string IdentityUserId { get; private set; }
        public ApplicationUser IdentityUser { get; private set; }
        public ICollection<LeaveRequest> LeaveRequests { get; private set; } = new List<LeaveRequest>();
        public ICollection<OvertimeRequest> OvertimeRequests { get; private set; } = new List<OvertimeRequest>();
        public ICollection<Resignation> Resignations { get; private set; } = new List<Resignation>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; private set; } = new List<AttendanceRecord>();
        public ICollection<LeaveEntitlement> LeaveEntitlements { get; private set; } = new List<LeaveEntitlement>();
        public ICollection<LeaveLedger> LeaveLedgerEntries { get; private set; } = new List<LeaveLedger>();
        public ICollection<Payroll> Payrolls { get; private set; } = new List<Payroll>();
    }
}
