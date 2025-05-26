using System.Diagnostics;
using App.E_Ticaret.Models;
using Microsoft.AspNetCore.Mvc;

namespace App.E_Ticaret.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Listing()
        {
            return View();
        }

       
    }
}
