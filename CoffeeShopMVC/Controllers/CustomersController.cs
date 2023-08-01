using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopMVC.Controllers
{
    public class CustomersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
