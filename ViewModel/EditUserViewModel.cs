using Microsoft.AspNetCore.Mvc.Rendering;

namespace MultiTenantTaskManager.ViewModel
{
    public class EditUserViewModel
    {
        public string? Id { get; set; }
        public string? Username { get; set; }
        public string? FullName { get; set; }
        public string? Email { get; set; }
        public string? TenantId { get; set; }
        public string? SelectedRole { get; set; }
        public List<SelectListItem>?  Roles { get; set; }
        public List<SelectListItem>? TenantIds { get; set; }
    }
}
