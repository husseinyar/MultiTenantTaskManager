using Microsoft.AspNetCore.Mvc;

namespace MultiTenantTaskManager.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
