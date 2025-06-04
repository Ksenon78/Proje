using System.Security.Claims;
using App.Data;
using App.Data.Entities;
using App.Data.Repositories;
using App.E_Ticaret.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace App.E_Ticaret.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IRepository<OrderEntity> _orderRepository;
        private readonly IRepository<OrderItemEntity> _orderItemRepository;
        private readonly IRepository<CartItemEntity> _cartItemRepository;
        private readonly IRepository<ProductEntity> _productRepository;
        private readonly AppDbContext _dbContext;

        public OrderController(
            IRepository<OrderEntity> orderRepository,
            IRepository<OrderItemEntity> orderItemRepository,
            IRepository<CartItemEntity> cartItemRepository,
            IRepository<ProductEntity> productRepository,
            AppDbContext dbContext)
        {
            _orderRepository = orderRepository;
            _orderItemRepository = orderItemRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
            _dbContext = dbContext;
        }


        [HttpGet]
        public async Task<IActionResult> Create()
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");


            var allCartItems = await _cartItemRepository.GetAllIncludingAsync(c => c.Product);
            var cartItems = allCartItems.Where(c => c.UserId == userId.Value).ToList();
            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");


            var vm = new OrderCreateViewModel
            {
                CartItems = cartItems
            };

            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Create(OrderCreateViewModel vm)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

           
            if (!ModelState.IsValid)
            {

                var allCartItems = await _cartItemRepository.GetAllIncludingAsync(c => c.Product);
                vm.CartItems = allCartItems.Where(c => c.UserId == userId.Value).ToList();
                return View(vm);
            }

      
            var freshCartItems = await _cartItemRepository.GetAllIncludingAsync(c => c.Product);
            var cartItems = freshCartItems.Where(c => c.UserId == userId.Value).ToList();
            if (!cartItems.Any())
                return RedirectToAction("Index", "Cart");

   
            var orderCode = Guid.NewGuid().ToString().Substring(0, 8).ToUpper();
            var order = new OrderEntity
            {
                OrderCode = orderCode,
                Address = vm.Address.Trim(),
                CreatedAt = DateTime.Now,
                UserId = userId.Value
            };

            await _orderRepository.AddAsync(order);
            await _dbContext.SaveChangesAsync(); 

           
            foreach (var ci in cartItems)
            {
                var item = new OrderItemEntity
                {
                    OrderId = order.Id,
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price,
                    CreatedAt = DateTime.Now
                };
                await _orderItemRepository.AddAsync(item);
            }

           
            foreach (var ci in cartItems)
            {
                _cartItemRepository.Remove(ci);
            }

            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = order.Id });
        }

        
        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            int? userId = GetUserId();
            if (userId == null)
                return RedirectToAction("Login", "Auth");

     
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null || order.UserId != userId.Value)
                return NotFound();

            var allOrderItems = await _orderItemRepository.GetAllIncludingAsync(oi => oi.Product);
            var orderItems = allOrderItems.Where(oi => oi.OrderId == order.Id).ToList();

            ViewData["OrderItems"] = orderItems;
            return View(order);
        }

        private int? GetUserId()
        {
            var claim = User.FindFirst(ClaimTypes.NameIdentifier);
            return claim == null ? (int?)null : int.Parse(claim.Value);
        }
    }
}
