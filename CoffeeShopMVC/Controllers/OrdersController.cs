using CoffeeShopMVC.DataAccess;
using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.Models;

namespace CoffeeShopMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CoffeeShopMVCContext _context;

        public OrdersController(CoffeeShopMVCContext context)
        {
            _context = context;
        }

        [HttpPost]
        public IActionResult Index(int id)
        {
            Order order = new Order();
            order.OrderCustomer.Id = id;

            _context.Orders.Add(order);
            _context.SaveChanges();
            var orderId = order.Id;

            return Redirect($"/orders/details/{orderId}");
        }

        [Route("/orders/details/{id:int}")]
        public IActionResult Details(int id)
        {
            var order = _context.Orders.Find(id);
            return View(order);
        }
    }
}
