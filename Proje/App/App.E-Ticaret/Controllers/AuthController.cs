using App.Data;
using App.Data.Entities;
using App.Data.Settings;
using App.E_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
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


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var existingUser = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email);

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Bu e-posta zaten kayıtlı.");
                return View(model);
            }

            var newUser = new UserEntity
            {
                Email = model.Email,
                Password = model.Password, 
                FirstName = "FirstNamePlaceholder",
                LastName = "LastNamePlaceholder",  
                CreatedAt = DateTime.UtcNow,
                RoleId = 1, 
                Enabled = true
            };

            _dbContext.Users.Add(newUser);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Login");
        }


        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _dbContext.Users
                .FirstOrDefaultAsync(u => u.Email == model.Email && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz e-posta veya şifre.");
                return View(model);
            }

            var claims = new List<Claim>
    {
           new Claim(ClaimTypes.Name, user.Email),
           new Claim("userId", user.Id.ToString())
    };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(Settings.AuthCookieName, principal);

            return RedirectToAction("Index", "Home");
        }




        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Auth");
        }
    }
}


