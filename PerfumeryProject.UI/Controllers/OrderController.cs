using Microsoft.AspNetCore.Mvc;

namespace PerfumeryProject.UI.Controllers
{
    public class OrderController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Detail(Guid orderId)
        {
            return View();
        }
    }
}
