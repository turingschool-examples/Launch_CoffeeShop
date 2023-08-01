﻿using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace CoffeeShopMVC.Controllers
{
    public class CustomersController : Controller
    {
        
        private readonly CoffeeShopMVCContext _context;

        public CustomersController(CoffeeShopMVCContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }


        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Route("/customer/create")]
        public IActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("index");
        }


    }
}
