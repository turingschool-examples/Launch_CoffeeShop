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
            var items = _context.Items;

            return View(items);
        }

        [Route("/Items/Details/{id:int}")]
        public IActionResult Show(int id)
        {
            var item = _context.Items.Find(id);
            return View(item);
        }
    }
}
