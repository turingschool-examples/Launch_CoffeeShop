﻿using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopMVC.Controllers
{
    public class OrdersController : Controller
    {
        private readonly CoffeeShopMVCContext _context;

        public OrdersController(CoffeeShopMVCContext context)
        {
            _context = context;
        }

        [Route("/customers/{customerId:int}/orders")]
        public IActionResult Index(int customerId)
        {
            var customer = _context.Customers.Where(c => c.Id == customerId).Include(c => c.Orders).ThenInclude(o => o.Items).First();
            return View(customer);
        }

        [Route("/customers/{customerId:int}/orders/details/{orderId:int}")]
        public IActionResult Show(int customerId, int orderId)
        {
            var customer = _context.Customers.Where(c => c.Id == customerId).Include(c => c.Orders).ThenInclude(o => o.Items).First();
            var order = customer.Orders.Where(o => o.Id == orderId).First();
            return View(order);
        }

        [Route("/customers/{customerId:int}/orders/edit/{orderId:int}")]
        public IActionResult Edit(int customerId, int orderId)
        {
            var customer = _context.Customers.Where(c => c.Id == customerId).Include(c => c.Orders).ThenInclude(o => o.Items).First();
            var order = customer.Orders.Where(o => o.Id == orderId).First();
            return View(order);
        }

        [HttpPost]
        [Route("/customers/{customerId:int}/orders/details/{orderId:int}")]
        public IActionResult Update(int customerId, int orderId, Order order)
        {
            order.Id = orderId;
            order.DateCreated = order.DateCreated.ToUniversalTime();
            _context.Orders.Update(order);
            _context.SaveChanges();
            return Redirect($"/customers/{customerId}/orders/details/{orderId}");
        }
    }
}
