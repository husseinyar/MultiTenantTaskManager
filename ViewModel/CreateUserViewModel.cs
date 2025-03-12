using Microsoft.AspNetCore.Mvc.Rendering;
using MultiTenantTaskManager.Models;

namespace MultiTenantTaskManager.ViewModel
{
    public class CreateUserViewModel
    {
        public RegisterModel ? RegisterModel { get; set; }
        public List<SelectListItem>? Roles { get; set; }
        public List<SelectListItem>? TenantIds { get; set; }
    }
}
