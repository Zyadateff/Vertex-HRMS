
using Microsoft.AspNetCore.Identity;

namespace VertexHRMS.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
            CreatedAt = DateTime.UtcNow;
        }

        public DateTime CreatedAt { get; set; }
        public Employee Employee { get; set; }
            public bool MustChangePassword { get; set; } = false;
        }
    }

