using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.Model;

namespace CoffeeShopMVC.Controllers
{
    public class CustomersController : Controller
    {
        private readonly CoffeeShopMVCContext _context;

        public CustomersController(CoffeeShopMVCContext Context)
        {
            _context = Context;
        }

        public IActionResult Index()
        {
            var customers = _context.Customers;

            return View(customers);
        }
    }
}