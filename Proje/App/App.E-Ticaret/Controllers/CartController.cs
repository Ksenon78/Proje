using Microsoft.AspNetCore.Mvc;
using App.Data.Entities;
using App.Data.Repositories;
using System.Security.Claims;
using App.Data;

namespace App.E_Ticaret.Controllers
{
    public class CartController : Controller
    {
        private readonly IRepository<CartItemEntity> _cartRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly AppDbContext _dbContext;

        public CartController(
            IRepository<CartItemEntity> cartRepository,
            IRepository<ProductEntity> productRepository,
            AppDbContext dbContext)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
            _dbContext = dbContext;
        }


        public async Task<IActionResult> Index()
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            
            var allCartItems = await _cartRepository.GetAllIncludingAsync(c => c.Product);
            var cartItems = allCartItems
                .Where(c => c.UserId == userId.Value)
                .ToList();

            return View(cartItems);
        }

    
        [HttpGet]
        public async Task<IActionResult> AddToCart()
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

            
            var allProducts = await _productRepository
                .GetAllIncludingAsync(p => p.Images, p => p.Category);

            return View(allProducts.ToList());
        }

     
        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

        
            var product = await _productRepository.GetByIdAsync(productId);
            if (product == null)
                return NotFound("Ürün bulunamadı.");

            var allItems = await _cartRepository.GetAllAsync();
            var existingCartItem = allItems
                .FirstOrDefault(c => c.UserId == userId.Value && c.ProductId == productId);

            if (existingCartItem != null)
            {
              
                existingCartItem.Quantity = (byte)(existingCartItem.Quantity + quantity);
                _cartRepository.Update(existingCartItem);
            }
            else
            {
                var newCartItem = new CartItemEntity
                {
                    UserId = userId.Value,
                    ProductId = productId,
                    Quantity = (byte)quantity,
                    CreatedAt = DateTime.UtcNow
                };
                await _cartRepository.AddAsync(newCartItem);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Cart");
        }


        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Index", "Home");

        
            var allItems = await _cartRepository.GetAllAsync();
            var cartItem = allItems.FirstOrDefault(c => c.Id == cartItemId && c.UserId == userId.Value);

            if (cartItem == null)
                return NotFound();

            _cartRepository.Remove(cartItem);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim == null ? (int?)null : int.Parse(claim.Value);
        }
    }
}
