using Microsoft.AspNetCore.Mvc;
using App.Data;
using Microsoft.EntityFrameworkCore;
using App.Data.Entities;
using Microsoft.CodeAnalysis;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace App.E_Ticaret.Controllers
{

    namespace App.E_Ticaret.Models.ViewModels
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
                int userId = GetUserId();

                var cartItems = await _dbContext.CartItems.Where(c => c.Id == userId).Include(c => c.Product).ToListAsync();
           
                return View(cartItems);
            
            }

      

            [HttpPost]

            [Authorize]
            public async Task<IActionResult> AddToCart(int productId, int quantity = 1)
            {
                 
                int userId = GetUserId();
                var product = await _dbContext.Products.FindAsync(productId);

                if(product == null)
                {
                    return NotFound("Ürün bulunamadı.");
                }

                var existingCartItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.UserId == userId && c.ProductId == productId); ;

                if (existingCartItem != null)
                {
                    existingCartItem.Quantity = (byte)(existingCartItem.Quantity + quantity);
                }

                else
                {
                    var newCartItem = new CartItemEntity
                    {
                        UserId = userId,
                        ProductId = productId,
                        Quantity = (byte)quantity,
                        CreatedAt = DateTime.UtcNow
                    };

                    await _dbContext.CartItems.AddAsync(newCartItem);

                }

                await _dbContext.SaveChangesAsync();
                return RedirectToAction("Index");

            }


            [HttpPost]

            public async Task<IActionResult> RemoveFromCart(int cartItemId)
            {
                int userId = GetUserId();

                var cartItem = await _dbContext.CartItems.FirstOrDefaultAsync(c => c.Id == cartItemId && c.UserId == userId);

                if (cartItem == null)
                {
                    return NotFound();
                }

                _dbContext.CartItems.Remove(cartItem);
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");


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
}
