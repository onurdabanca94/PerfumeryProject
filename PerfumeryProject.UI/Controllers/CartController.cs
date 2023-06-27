using Microsoft.AspNetCore.Mvc;

namespace PerfumeryProject.UI.Controllers
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
