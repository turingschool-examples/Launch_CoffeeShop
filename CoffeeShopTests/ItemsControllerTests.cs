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

            
            Assert.DoesNotContain("Coffeeeee", html);
        }
        [Fact]
        public async Task New_ShowsNewForm()
        {
            var client = _factory.CreateClient();

            var response = await client.GetAsync("/Items/new");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("<form method=\"post\" action=\"/items\">", html);
            Assert.Contains("<button type=\"submit\" >Add Item</button>", html);
            Assert.Contains("Name", html);
            Assert.Contains("Price", html);

        }

        [Fact]
        public async Task Create_AddsItemToDB()
        {
            var client = _factory.CreateClient();

            var addItemFormData = new Dictionary<string, string>
            {
                {"Name", "Dirt" },
                {"PriceInCents", "100" }
            };
            
            var response = await client.PostAsync("/items", new FormUrlEncodedContent(addItemFormData));
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Dirt", html);
            Assert.Contains("1", html);
        }
    }
}

