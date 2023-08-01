using CoffeeShopMVC.DataAccess;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CoffeeShopMVCContext _context;

        public OrdersController(CoffeeShopMVCContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
