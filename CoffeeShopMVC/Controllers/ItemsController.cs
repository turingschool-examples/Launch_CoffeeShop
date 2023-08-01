﻿using Microsoft.AspNetCore.Mvc;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;

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

        [Route("/items/details/{Id:int}")]
        public IActionResult Details(int Id)
        {
            var item = _context.Items.Find(Id);
            return View(item);
        }

        public IActionResult New()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(Item item)
        {
          
            _context.Items.Add(item);
            _context.SaveChanges();
              var newItemId = item.Id;

           // return Redirect($"/items/details/{item.Id}");
            return RedirectToAction("details", new {id = newItemId});
        }

        [HttpPost]
        public IActionResult Delete(int Id)
        {
            var item = _context.Items.Find(Id);
            _context.Items.Remove(item);
            _context.SaveChanges();

            return Redirect("/items");
        }

        [Route("/items/{id:int}/edit")]
        public IActionResult Edit(int Id)
        {
            var item = _context.Items.Find(Id);
            return View(item);
        }

        [HttpPost]
        [Route("/items/{id:int}")]
        public IActionResult Update(int Id, Item item)
        {
            item.Id = Id;
            _context.Items.Update(item);
            _context.SaveChanges();

            return RedirectToAction("details", new {Id = item.Id});
        }
    }
}