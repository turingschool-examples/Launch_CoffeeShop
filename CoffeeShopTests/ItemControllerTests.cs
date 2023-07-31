using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTests
{
    public class ItemsControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        
        public ItemsControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task Index_ReturnViewWithAllItems()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item item1 = new Item { Name = "Coffee", PriceInCents = 299 };
            Item item2 = new Item { Name = "Donut", PriceInCents = 199 };
            context.Items.Add(item1);
            context.Items.Add(item2);
            context.SaveChanges();

            var response = await client.GetAsync("/items");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains(item1.Name, html);
            Assert.Contains(item2.Name, html);
            Assert.DoesNotContain(item2.PriceInCents.ToString(), html);

        }

        [Fact]
        public async Task Show_ReturnsOneItemDetails()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item item1 = new Item { Name = "Coffee", PriceInCents = 299 };
            Item item2 = new Item { Name = "Donut", PriceInCents = 199 };
            context.Items.Add(item1);
            context.Items.Add(item2);
            context.SaveChanges();

            var response = await client.GetAsync($"/items/details/{item1.Id}");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains(item1.Name, html);
            Assert.DoesNotContain(item2.Name, html);
            Assert.Contains("$2.99", html);

        }

        [Fact]
        public async Task Delete_DeletesOneItem()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item item1 = new Item { Name = "Frape", PriceInCents = 299 };
            Item item2 = new Item { Name = "Donut", PriceInCents = 199 };
            context.Items.Add(item1);
            context.Items.Add(item2);
            context.SaveChanges();

            var formdata = new Dictionary<string, string>();

            var response = await client.PostAsync($"/items/details/{item1.Id}/delete", new FormUrlEncodedContent(formdata));
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();
            Assert.DoesNotContain(item1.Name, html);
            Assert.Contains(item2.Name, html);
        }
    }
}