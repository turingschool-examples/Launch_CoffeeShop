using CoffeeShopMVC.DataAccess;
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
            var items = _context.Items.ToList();
            return View(items);
        }

        [Route("/items/details/{id:int}")]
        public IActionResult Show(int id)
        {
            var item = _context.Items.Find(id);

            return View(item);
        }

        [HttpPost]
        [Route("/items/details/{id:int}/delete")]
        public IActionResult Delete(int id)
        {
            var item = _context.Items.Find(id);
            _context.Items.Remove(item);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
