using Microsoft.AspNetCore.Mvc;
using App.Data;
using Microsoft.EntityFrameworkCore;
using App.Data.Entities;
using System.Security.Claims;

namespace App.E_Ticaret.Controllers
{
    public class CartController : Controller
    {
        private readonly AppDbContext _dbContext;

        public CartController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cartItems = await _dbContext.CartItems
                .Where(c => c.UserId == userId)
                .Include(c => c.Product)
                .ToListAsync();

            return View(cartItems);
        }


        [HttpGet]
        public async Task<IActionResult> AddToCart()
        {
            var products = await _dbContext.Products
                .Include(p => p.Images)
                .Include(p => p.Category)
                .ToListAsync();

            return View(products);  
        }

        [HttpPost]
        public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
        {
            int? userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var product = await _dbContext.Products.FindAsync(productId);

            if (product == null)
            {
                return NotFound("Ürün bulunamadı.");
            }

            var existingCartItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity = (byte)(existingCartItem.Quantity + quantity);
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

                await _dbContext.CartItems.AddAsync(newCartItem);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction("Index", "Cart");
        }

        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            int? userId = GetUserId();

            if (userId == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var cartItem = await _dbContext.CartItems
                .FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _dbContext.CartItems.Remove(cartItem);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction("Index", "Cart");
        }

        private int? GetUserId()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return null;
            }

            return int.Parse(userIdClaim.Value);
        }
    }
}
