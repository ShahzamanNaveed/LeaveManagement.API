using Microsoft.AspNetCore.Identity;

namespace LeaveManagement.API.Models
{
    public class ApplicationUser : IdentityUser
    {
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; }
            = DateTime.UtcNow;


        public Employee? Employee { get; set; }
    }
}