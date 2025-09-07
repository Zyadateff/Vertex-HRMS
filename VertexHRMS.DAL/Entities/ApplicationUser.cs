

namespace VertexHRMS.DAL.Entities
{
    public class ApplicationUser: IdentityUser
    {
        public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;
        public Employee Employee { get; private set; }
    }
}
