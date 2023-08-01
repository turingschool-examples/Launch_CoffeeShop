using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopMVC.Controllers
{
    public class CustomerController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
