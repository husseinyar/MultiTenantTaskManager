using Microsoft.AspNetCore.Identity;

namespace MultiTenantTaskManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string? TenantId { get; set; }
    }
}
