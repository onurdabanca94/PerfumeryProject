using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using PerfumeryProject.UI.Models;
using System.Diagnostics;

namespace PerfumeryProject.UI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IConfiguration Configuration;

        public HomeController(ILogger<HomeController> logger, IConfiguration configuration)
        {
            _logger = logger;
            Configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        
        public IActionResult AddParfume()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> GetFilterDataAsync(string searchText , string orderBy, int brandId)
        {
            ViewBag.searchText = searchText;
            ViewBag.orderBy = orderBy;
            ViewBag.brandId = brandId;

            return Json("Başarılı");
        }

        public IActionResult ParfumeDetail(int? parfumeId)
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}