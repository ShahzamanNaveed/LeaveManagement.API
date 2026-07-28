using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.API.Domain.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;


        public Employee? Employee { get; set; }
    }
}