using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using System.Runtime.CompilerServices;
 

namespace CoffeeShopTests
{

    [Collection("States Controller Tests")]
    public class CoffeeShopMVCTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CoffeeShopMVCTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }
        private CoffeeShopMVCContext GetDbContext()
        {
            var optionsBuilder = new DbContextOptionsBuilder<CoffeeShopMVCContext>();
            optionsBuilder.UseInMemoryDatabase("TestDatabase");

            var context = new CoffeeShopMVCContext(optionsBuilder.Options);
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            return context;
        }
        [Fact]
        public async Task Index_ShowsItems()
        {
            var context = GetDbContext();
            

            context.Items.Add(new Item { Name = "Dirt", PriceInCents = 10000 });
            context.Items.Add(new Item { Name = "Sand", PriceInCents = 5000 });
            context.SaveChanges();
            
            var client = _factory.CreateClient();
            
            var response = await client.GetAsync("/Items");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Dirt", html);
            Assert.Contains("Sand", html);

            // Make sure it does not hit actual database
            Assert.DoesNotContain("Coffeeeee", html);
        }
    }
}

