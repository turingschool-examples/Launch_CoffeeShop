using CoffeeShopMVC.FeatureTests;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;

namespace CoffeeShopTests
{
    public class ItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {

        private readonly WebApplicationFactory<Program> _factory;

        public ItemsControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async void Index_ReturnsViewOfListOfItems()
        {
            var context = GetDbContext();
            context.Items.Add(new Item { Name = "Latte", PriceInCents = 600 });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/items");
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Latte", html);
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
    }
}