using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.Model;

namespace CoffeeShopTests.FeatureTests
{
    public class ItemControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ItemControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task Test_Index_ReturnsViewWithItems()
        {
            var context = GetDbContext();
            context.Items.Add(new Item { Name = "Coffee Machine", PriceInCents = 1000 });
            context.Items.Add(new Item { Name = "Coffee Grinder", PriceInCents = 250 });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Items");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("Coffee Machine", html);
            Assert.Contains("Coffee Grinder", html);
        }
    }
}