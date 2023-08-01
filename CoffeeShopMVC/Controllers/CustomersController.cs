﻿using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            var customers = _context.Customers.ToList();
            return View(customers);
        }


        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        [Route("/customers/create")]
        public IActionResult Create(Customer customer)
        {
            _context.Customers.Add(customer);
            _context.SaveChanges();

            return RedirectToAction("index");
        }

        [Route("/customers/details/{id:int}")]
        public IActionResult Show(int id)
        {
            var customer = _context.Customers.Include(c => c.Orders).Where(c => c.Id == id).Single();

            return View(customer);
        }

    }
}
