using CoffeeShopMVC.DataAccess;
using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.Model;

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

        // GET: /Items/Details/id/edit
        [Route("/Items/Details/{id:int}/edit")]
        public IActionResult Edit(int id)
        {
            var item = _context.Items.Find(id);

            return View(item);
        }

        // PUT: /Items/Details/id
        [HttpPost]
        [Route("/Items/Details/{id:int}")]
        public IActionResult Update(Item item)
        {
            _context.Items.Update(item);
            _context.SaveChanges();

            return RedirectToAction("show", new { id = item.Id });
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
    }
}
