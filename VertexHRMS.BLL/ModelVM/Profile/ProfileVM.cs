
using System.ComponentModel.DataAnnotations;

namespace VertexHRMS.BLL.ModelVM
{
    public class ProfileVM
    {
        public int EmployeeId { get; set; }

        public string? ImagePath { get; set; }

        [Required, StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Required, EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Phone]
        public string Phone { get; set; } = string.Empty;

        public string DepartmentName { get; set; } = string.Empty;
        public string PositionName { get; set; } = string.Empty;
        public string? ManagerName { get; set; }

        // For other features
        public DateTime HireDate { get; set; }

        public DateTime? LastLogin { get; set; }
        public DateTime? LastUpdated { get; set; }
        public int TasksCount { get; set; }
        public int ProjectsCount { get; set; }
    }
}
