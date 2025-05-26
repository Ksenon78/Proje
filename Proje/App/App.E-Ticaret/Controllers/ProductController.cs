using System.Security.Claims;
using App.Data;
using App.Data.Entities;
using App.E_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace App.E_Ticaret.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _dbContext;

        public ProductController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Create()
        {
            var model = new ProductViewModel();

            model.CategoryList = _dbContext.Categories.Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToList();

            return View(model);
        }
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.CategoryList = await _dbContext.Categories
                    .Select(c => new CategoryDto { Id = c.Id, Name = c.Name }).ToListAsync();

                return View(model);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
                return Unauthorized();

            var sellerId = int.Parse(userIdClaim.Value);

            var product = new ProductEntity
            {
                Name = model.Name,
                Details = model.Details,
                Price = model.Price,
                Description = model.Description,
                StockAmount = model.StockAmount,
                CategoryId = model.CategoryId,
                SellerId = sellerId,
            };

            _dbContext.Product.Add(product);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
