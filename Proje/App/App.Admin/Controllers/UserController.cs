using System.Threading.Tasks;
using App.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {

        private readonly AppDbContext _dbContext;

        public UserController(AppDbContext dbContext)
        {
            _dbContext = dbContext;

        }
        

        public async Task<IActionResult> Index()
        {
            var users = await _dbContext.Users
                         .Include(u => u.Role)
                          .ToListAsync();

            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> Approve(int id)
        {
            var user = await _dbContext.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            var sellerRole = await _dbContext.Roles.FirstOrDefaultAsync(r => r.Name == "Seller");
            if(sellerRole == null)
            {
                return BadRequest("Seller rolü bulunamadı");
            }
            
            user.RoleId = sellerRole.Id;   

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");

        }
        [HttpGet]
        public IActionResult List()
        {
            return View();
        }

       
      
    }
}