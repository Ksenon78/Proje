using App.Data;
using App.Data.Entities;
using App.Data.Repositories;
using App.Data.Settings;
using App.E_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace App.E_Ticaret.Controllers
{
    public class AuthController : Controller
    {
        private readonly IRepository<UserEntity> _userRepository;
        private readonly IRepository<RoleEntity> _roleRepository;
        private readonly AppDbContext _dbContext; 

        public AuthController(
            IRepository<UserEntity> userRepository,
            IRepository<RoleEntity> roleRepository,
            AppDbContext dbContext)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
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

            var allUsers = await _userRepository.GetAllAsync();
            var existingUser = allUsers.FirstOrDefault(u =>
                u.Email.Trim().ToLower() == model.Email.Trim().ToLower());

            if (existingUser != null)
            {
                ModelState.AddModelError("", "Bu e-posta zaten kayıtlı.");
                return View(model);
            }

            var allRoles = await _roleRepository.GetAllAsync();
            var adminRole = allRoles.FirstOrDefault(r => r.Name == "Admin");
            var buyerRole = allRoles.FirstOrDefault(r => r.Name == "Buyer");

            if (adminRole == null || buyerRole == null)
            {
                ModelState.AddModelError("", "Gerekli roller veritabanında bulunamadı.");
                return View(model);
            }

            var adminEmailToMatch = "admin@example.com";
            var roleToAssign = model.Email.Trim().ToLower() == adminEmailToMatch
                ? adminRole
                : buyerRole;

            var newUser = new UserEntity
            {
                Email = model.Email.Trim(),
                Password = model.Password, 
                FirstName = "FirstNamePlaceholder",
                LastName = "LastNamePlaceholder",
                CreatedAt = DateTime.UtcNow,
                RoleId = roleToAssign.Id,
                Enabled = true
            };

            await _userRepository.AddAsync(newUser);
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

            var allUsers = await _userRepository.GetAllIncludingAsync(u => u.Role);
            var user = allUsers.FirstOrDefault(u =>
                u.Email.Trim().ToLower() == model.Email.Trim().ToLower()
                && u.Password == model.Password);

            if (user == null)
            {
                ModelState.AddModelError("", "Geçersiz e-posta veya şifre.");
                return View(model);
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Email),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),

                
                new Claim("userId", user.Id.ToString()),


                new Claim(ClaimTypes.Role, user.Role?.Name ?? "Buyer")
            };

            var identity = new ClaimsIdentity(
                claims,
                CookieAuthenticationDefaults.AuthenticationScheme);

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(
                Settings.AuthCookieName, 
                principal,
                new AuthenticationProperties
                {
                    IsPersistent = model.RememberMe,
                    ExpiresUtc = DateTimeOffset.UtcNow.AddHours(1)
                });

            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {

            await HttpContext.SignOutAsync(Settings.AuthCookieName);
            return RedirectToAction("Login", "Auth");
        }


        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim == null ? (int?)null : int.Parse(claim.Value);
        }
    }
}
