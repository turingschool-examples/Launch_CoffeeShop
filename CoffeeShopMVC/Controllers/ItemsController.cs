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
        [Route("/Items/details/{itemId:int}")]
        public IActionResult Show(int itemId)
        {
            var item = _context.Items.Find(itemId);
            return View(item);
        }
    }
}
