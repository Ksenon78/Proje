using App.Data;
using App.Data.Settings;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.Tasks;

namespace App.E_Ticaret.Controllers
{
    public class AuthController : Controller
    {
        private readonly AppDbContext _dbContext;


        public AuthController(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public IActionResult Index()
        {
            return View();
        }

        [Route("/register")]
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromForm] LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = UserManager.Login(model.Email, model.Password);

            if (user != null)
            {
                var identity = new ClaimsIdentity(user.Claims, Settings.AuthCookieName);
                var principal = new ClaimsPrincipal(identity);

                await HttpContext.SignInAsync(Settings.AuthCookieName, principal);

                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError(string.Empty, "Geçersiz kullanıcı adı veya şifre.");
            return View(model);
        }
    }
}