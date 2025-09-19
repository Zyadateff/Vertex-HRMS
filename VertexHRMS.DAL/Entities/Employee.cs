namespace VertexHRMS.DAL.Entities
{
    public class Employee
    {
        public Employee()
        {
            DirectReports = new List<Employee>();
            LeaveRequests = new List<LeaveRequest>();
            OvertimeRequests = new List<OvertimeRequest>();
            Resignations = new List<Resignation>();
            AttendanceRecords = new List<AttendanceRecord>();
            LeaveEntitlements = new List<LeaveEntitlement>();
            LeaveLedgerEntries = new List<LeaveLedger>();
            Payrolls = new List<Payroll>();
        }

        public int EmployeeId { get; set; }
        public string ImagePath { get; set; }
        public string EmployeeCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime HireDate { get; set; }
        public string EmploymentType { get; set; }
        public string Status { get; set; }
        public int DepartmentId { get; set; }
        public Department Department { get; set; }
        public int PositionID { get; set; }
        public Position Position { get; set; }
        public int? ManagerId { get; set; }
        public Employee Manager { get; set; }
        public ICollection<Employee> DirectReports { get; set; }
        public string IdentityUserId { get; set; }
        public ApplicationUser IdentityUser { get; set; }
        public ICollection<LeaveRequest> LeaveRequests { get; set; }
        public ICollection<OvertimeRequest> OvertimeRequests { get; set; }
        public ICollection<Resignation> Resignations { get; set; }
        public ICollection<AttendanceRecord> AttendanceRecords { get; set; }
        public ICollection<LeaveEntitlement> LeaveEntitlements { get; set; }
        public ICollection<LeaveLedger> LeaveLedgerEntries { get; set; }
        public ICollection<Payroll> Payrolls { get; set; }

        // Factory method for creating new employees
        public static Employee Create(
            string employeeCode,
            string firstName,
            string lastName,
            string email,
            string phone,
            int departmentId,
            int positionId,
            string identityUserId,
            string imagePath = null,
            int? managerId = null)
        {
            return new Employee
            {
                EmployeeCode = employeeCode,
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                Phone = phone,
                DepartmentId = departmentId,
                PositionID = positionId,
                IdentityUserId = identityUserId,
                ImagePath = imagePath,
                ManagerId = managerId,
                HireDate = DateTime.UtcNow,
                EmploymentType = "FullTime",
                Status = "Active"
            };
        }

        // Helper method to update employee info
        public void UpdateInfo(
            string firstName,
            string lastName,
            string phone,
            int departmentId,
            int positionId,
            string imagePath = null,
            int? managerId = null)
        {
            FirstName = firstName;
            LastName = lastName;
            Phone = phone;
            DepartmentId = departmentId;
            PositionID = positionId;
            if (!string.IsNullOrEmpty(imagePath))
                ImagePath = imagePath;
            ManagerId = managerId;
        }
    }
}