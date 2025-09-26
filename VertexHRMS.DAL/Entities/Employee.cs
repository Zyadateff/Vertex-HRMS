using System.ComponentModel.DataAnnotations.Schema;

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
        public Employee(string imagePath, string employeeCode, string firstName, string lastName,
                    string email, string phone, DateTime hireDate, string employmentType,
                    string status, int departmentId, int positionId, string identityUserId,
                    int? managerId = null)
        {
            ImagePath = imagePath;
            EmployeeCode = employeeCode;
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            Phone = phone;
            HireDate = hireDate;
            EmploymentType = employmentType;
            Status = status;
            DepartmentId = departmentId;
            PositionID = positionId;
            IdentityUserId = identityUserId;
            ManagerId = managerId;
        }
        public int EmployeeId { get; private set; }
        public string ImagePath { get; private set; }
        public string EmployeeCode { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string Email { get; private set; }
        public string Phone { get; private set; }
        public double? Salary { get; private set; }
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
        [ForeignKey(nameof(IdentityUser))]
        public string IdentityUserId { get; private set; }
        public ApplicationUser IdentityUser { get; private set; }
        public ICollection<LeaveRequest> LeaveRequests { get; private set; } = new List<LeaveRequest>();
        public ICollection<OvertimeRequest> OvertimeRequests { get; private set; } = new List<OvertimeRequest>();
        public ICollection<Resignation> Resignations { get; private set; } = new List<Resignation>();
        public ICollection<AttendanceRecord> AttendanceRecords { get; private set; } = new List<AttendanceRecord>();
        public ICollection<LeaveEntitlement> LeaveEntitlements { get; private set; } = new List<LeaveEntitlement>();
        public ICollection<LeaveLedger> LeaveLedgerEntries { get; private set; } = new List<LeaveLedger>();
        public ICollection<Payroll> Payrolls { get; private set; } = new List<Payroll>();
        public ICollection<ProjectTask> Tasks { get; set; }
        public ICollection<EmployeeTraining> Trainings { get; set; }
    }
}