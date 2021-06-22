using BuildingMaterialRent.Data;
using BuildingMaterialRent.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BuildingMaterialRent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly RentDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly IOptions<ApiBehaviorOptions> _apiBehaviorOptions;

        public OrdersController(
            RentDbContext context,
            UserManager<User> userManager,
            IOptions<ApiBehaviorOptions> apiBehaviorOptions)
        {
            _context = context;
            _userManager = userManager;
            _apiBehaviorOptions = apiBehaviorOptions;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public List<Order> Get()
        {
            var orders = _context
                .Orders
                .Include(o => o.User)
                .Where(o => o.Status != OrderStatus.Basket)
                .ToList();
            foreach (var order in orders)
            {
                order.User.Orders = null;
            }
            return orders;
        }

        [HttpGet]
        [Authorize]
        [Route("[action]")]
        public Order Basket()
        {
            var userName = User.Identity.Name;
            return GetBasket(userName);
        }

        [HttpPost]
        [Authorize]
        [Route("[action]")]
        public IActionResult Update([FromBody] UpdateOrderItem model)
        {
            if (!ValidateUser(model.OrderItemId))
            {
                return Forbid();
            }
            var orderItem = _context.OrderItems.Include(oi => oi.Product).Single(oi => oi.OrderItemId == model.OrderItemId);
            var currentQuantity = orderItem.Quantity;
            orderItem.Product.Stock = orderItem.Product.Stock + currentQuantity - model.Quantity;
            if (orderItem.Product.Stock < 0)
            {
                return BadRequest();
            }
            orderItem.Quantity = model.Quantity;
            _context.SaveChanges();
            return Ok();
        }

        [HttpPost]
        [Authorize]
        [Route("[action]/{id:int}")]
        public IActionResult DeleteItem(int id)
        {
            if (!ValidateUser(id))
            {
                return Forbid();
            }
            var orderItem = _context.OrderItems.Include(oi => oi.Product).Single(oi => oi.OrderItemId == id);
            orderItem.Product.Stock += orderItem.Quantity;
            _context.Remove(orderItem);
            _context.SaveChanges();
            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public IActionResult Add([FromBody] AddToOrderModel model)
        {
            var product = _context.Products.SingleOrDefault(p => p.ProductId == model.ProductId);
            if (product == null)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Invalid product ID");
                _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }
            if (product.Stock < model.Quantity)
            {
                ModelState.AddModelError(nameof(model.ProductId), "Not enough products on stock");
                _apiBehaviorOptions.Value.InvalidModelStateResponseFactory(ControllerContext);
            }
            var userName = User.Identity.Name;
            var basket = GetBasket(userName);
            product.Stock -= model.Quantity;
            basket.OrderItems ??= new List<OrderItem>();
            basket.OrderItems.Add(new OrderItem
            {
                Quantity = model.Quantity,
                ProductId = product.ProductId,
                Price = model.Quantity * product.Price,
                OrderId = basket.OrderId,
            });
            RecalculateTotal(basket);
            _context.SaveChanges();

            return Ok();
        }

        [Authorize]
        [HttpPost]
        [Route("[action]")]
        public IActionResult PlaceOrder()
        {
            var basket = GetBasket(User.Identity.Name);
            if (!basket?.OrderItems?.Any() ?? true)
            {
                return BadRequest();
            }
            basket.Status = OrderStatus.OrderCreated;
            basket.OrderDate = DateTime.Now;
            _context.SaveChanges();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("[action]/{id:int}")]
        public IActionResult PickupOrder(int id)
        {
            var order = _context.Orders.FirstOrDefault(o => o.OrderId == id);
            if (order == null || order.Status != OrderStatus.OrderCreated)
            {
                return BadRequest();
            }
            order.Status = OrderStatus.OrderPickedUp;
            order.PickupDate = DateTime.Now;
            _context.SaveChanges();
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Route("[action]/{id:int}")]
        public IActionResult ReturnOrder(int id)
        {
            var order = _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.OrderId == id);
            if (order == null || order.Status != OrderStatus.OrderPickedUp)
            {
                return BadRequest();
            }
            order.Status = OrderStatus.OrderReturned;
            order.DueDate = DateTime.Now;

            foreach (var item in order.OrderItems)
            {
                item.Product.Stock += item.Quantity;
            }
            _context.SaveChanges();
            return Ok();
        }

        private Order GetBasket(string userName)
        {
            var basket = _context
                .Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Product)
                .FirstOrDefault(o => o.Status == OrderStatus.Basket && o.User.UserName == userName);
            if (basket == null)
            {
                var userId = _userManager.GetUserId(User);
                basket = new Order
                {
                    Status = OrderStatus.Basket,
                    Total = 0m,
                    UserId = userId
                };
                _context.Orders.Add(basket);
                _context.SaveChanges();
            }
            return basket;
        }

        private void RecalculateTotal(Order order)
        {
            order.Total = order.OrderItems?.Sum(oi => oi.Price * oi.Quantity) ?? 0;
        }

        private bool ValidateUser(int id)
        {
            var user = _context.Users.FirstOrDefault(u => u.Orders.Any(o => o.OrderItems.Any(oi => oi.OrderItemId == id)));
            if (user == null || User.Identity.Name != user.UserName)
            {
                return false;
            }
            return true;
        }
    }
}
