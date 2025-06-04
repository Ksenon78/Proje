using System.Security.Claims;
using App.Data;
using App.Data.Entities;
using App.Data.Repositories;
using App.E_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace App.E_Ticaret.Controllers
{
    [Authorize]

    public class ProductController : Controller
    {
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly IRepository<CategoryEntity>   _categoryRepository;
        private readonly AppDbContext _dbContext;

        public ProductController( IRepository<ProductEntity> productRepository,
               IRepository<CategoryEntity> categoryRepository,  
               AppDbContext dbContext)
        {
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;  
            _dbContext = dbContext;
        }
        public async Task<IActionResult> Index()
        {
            var products = await _productRepository.GetAllIncludingAsync(p => p.Images);
            return View(products);
        }

        public async Task<IActionResult> Create()
        {
            var model = new ProductViewModel();
            var categories = await _categoryRepository.GetAllAsync();

            model.CategoryList = categories
                .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
                .ToList();

            return View(model);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromForm] ProductViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryRepository.GetAllAsync();
                model.CategoryList = categories
                    .Select(c => new CategoryDto { Id = c.Id, Name = c.Name })
                    .ToList();

                return View(model);
            }

            var sellerId = GetUserId();

            var product = new ProductEntity
            {
                Name = model.Name,
                Details = model.Details,
                Price = model.Price,
                Description = model.Description,
                StockAmount = model.StockAmount,
                CategoryId = model.CategoryId,
                SellerId = sellerId,
                Enabled = true,
                CreatedAt = DateTime.UtcNow
            };

            await _productRepository.AddAsync(product);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Listing()
        {
            var products = await _productRepository.GetAllIncludingAsync(p => p.Images, p => p.Category);

            return View(products);
        }

        private int GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                throw new Exception("Kullanıcının bu işlem için yetkisi yok");
            }

            return int.Parse(userIdClaim.Value);
        }

    }
}




