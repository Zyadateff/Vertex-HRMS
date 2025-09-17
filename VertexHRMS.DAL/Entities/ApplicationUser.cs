

namespace VertexHRMS.DAL.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public ApplicationUser()
        {

        }
        public ApplicationUser(Employee employee = null)
        {
            Employee = employee;
        }
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Employee Employee { get; private set; }
    }
}
