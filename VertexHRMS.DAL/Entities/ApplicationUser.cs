using Microsoft.AspNetCore.Identity;

namespace VertexHRMS.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedAt = DateTime.Now;
        }
        public ApplicationUser(Employee employee = null)
        {
            Employee = employee;
        }
        public bool MustChangePassword { get; set; } = false;
        public DateTime CreatedAt { get; private set; } = DateTime.Now;
        public Employee Employee { get; private set; }
    }
}