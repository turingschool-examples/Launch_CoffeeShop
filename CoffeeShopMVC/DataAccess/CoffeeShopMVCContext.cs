using CoffeeShopMVC.Models;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopMVC.DataAccess
{
    public class CoffeeShopMVCContext : DbContext 
    {
        public DbSet<Item> Items { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Customer> Customers { get; set; }

        public CoffeeShopMVCContext(DbContextOptions<CoffeeShopMVCContext> options) : base(options)
        {

        }
    }
}
