using Microsoft.EntityFrameworkCore;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.Controllers;
using CoffeeShopMVC.Model;

namespace CoffeeShopMVC.DataAccess
{
    public class CoffeeShopMVCContext : DbContext
    {
        public DbSet<Item> Items { get; set; }

        public CoffeeShopMVCContext(DbContextOptions<CoffeeShopMVCContext> options) : base(options) 
        {
        
        }
    }
}
