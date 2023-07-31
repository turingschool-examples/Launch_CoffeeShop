using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;

namespace CoffeeShopMVC.Controllers
{
    public class ItemsController : Controller
    {
        private readonly CoffeeShopMVCContext _context;

        public ItemsController(CoffeeShopMVCContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var items = _context.Items; 
            return View(items);
        }

        public IActionResult New()
        {
            return View();
        }

        public IActionResult Create(Item item)
        {
            _context.Items.Add(item);
            _context.SaveChanges();
            var newItemId = item.Id;

            return Redirect($"/items/{newItemId}");
            //return RedirectToAction("show", new {id = newItemId});
        }
    }
}