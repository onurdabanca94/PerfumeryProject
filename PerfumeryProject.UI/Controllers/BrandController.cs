using Microsoft.AspNetCore.Mvc;

namespace PerfumeryProject.UI.Controllers
{
    public class BrandController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
