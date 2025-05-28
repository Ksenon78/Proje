using System.Security.Claims;
using App.Data;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace App.E_Ticaret.Controllers
{
    public class OrderController : Controller
    {

        private readonly AppDbContext _dbContext;

        public OrderController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]

        public async Task<IActionResult> Create()
        {

            int? userId = GetUserId();
            if (userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var cartItems = await _dbContext.CartItems.Include(c => c.Product).Where(c => c.UserId == userId).ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }

            ViewData["CartItems"] = cartItems;
            return View(new OrderEntity());

        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Address")] OrderEntity form)
        {
            int? userId = GetUserId();

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return RedirectToAction("Login", "Auth");
            }


            var cartItems = await _dbContext.CartItems.Include(c => c.Product)
                .Where(c =>  c.Id == userId).ToListAsync();

            if (!cartItems.Any())
            {
                return RedirectToAction("Index", "Cart");
            }


            var orderCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            var order = new OrderEntity
            {
                OrderCode = orderCode,
                Address = form.Address,
                CreatedAt = DateTime.Now,
                UserId = userId.Value
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            foreach( var property in cartItems )
            {
                var item = new OrderItemEntity
                {
                    OrderId = order.Id,
                    ProductId = property.ProductId,
                    Quantity = property.Quantity,
                    UnitPrice = property.Product.Price,
                    CreatedAt = DateTime.Now,
                };

                _dbContext.OrderItems.Add(item);

            }

            _dbContext.CartItems.RemoveRange(cartItems);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            int? userId = GetUserId();

            if(userId == null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var order = await _dbContext.Orders.Include(o => o.OrderItems).
                ThenInclude(o => o.Product).FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);


            if (order == null)
            {
                return NotFound();
            }

            return View(order);

        }


        private int? GetUserId()
        {
            var c = User.FindFirst(ClaimTypes.NameIdentifier);
            return c == null ? (int?)null : int.Parse(c.Value);
        }
    }
}
