using Microsoft.AspNetCore.Mvc;

namespace App.E_Ticaret.Models.ViewModels
{
    public class CartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
