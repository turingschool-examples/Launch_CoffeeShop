using CoffeeShopMVC.DataAccess;
using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.Models;

namespace CoffeeShopMVC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CoffeeShopMVCContext _context;

        public CustomersController(CoffeeShopMVCContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult New()
        {
            return View();
        }

        [Route("Customers/{id:int}")]
        public IActionResult Details(int id)
        {
            var customer = _context.Customers.Find(id);
            return View(customer);
        }

    }
}
