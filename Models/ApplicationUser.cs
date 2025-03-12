using Microsoft.AspNetCore.Identity;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace MultiTenantTaskManager.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
    
        public string? TenantId { get; set; }

    }
}
