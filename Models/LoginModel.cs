using System.ComponentModel.DataAnnotations;

namespace MultiTenantTaskManager.Models
{
    public class LoginModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
