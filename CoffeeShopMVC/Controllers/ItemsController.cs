using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Model;
using Microsoft.AspNetCore.Mvc;

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

        [Route("/Items/Details/{id:int}")]
        public IActionResult Show(int id)
        {
            var item = _context.Items.Find(id);
            return View(item);
        }

        [HttpPost]
        [Route("/Items/Details/Delete/{id:int}")]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.Find(id);

            _context.Items.Remove(item);
            _context.SaveChanges();

            return RedirectToAction("Index");
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Route("/items/")]
        public IActionResult Create(Item item)
        {
            _context.Add(item);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

    }
}
