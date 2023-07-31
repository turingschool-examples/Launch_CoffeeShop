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

        [Route("/items/details/{Id:int}")]
        public IActionResult Details(int Id)
        {
            var item = _context.Items.Find(Id);
            return View(item);

        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Item item)
        {
           // var newItemId = item.Id;
            _context.Items.Add(item);
            _context.SaveChanges();
            

            return Redirect($"/items/details/{item.Id}");
            //return RedirectToAction("show", new {id = newItemId});
        }
        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var item = _context.Items.Find(Id);
            _context.Items.Remove(item);
            _context.SaveChanges();

            return Redirect("/items");
        }
    }
}