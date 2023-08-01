using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.Model;

namespace CoffeeShopTests.FeatureTests
{
    [Collection("Controller Tests")]
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

        [Fact]
        public async Task Test_Show_ReturnsItemDetails()
        {
            var context = GetDbContext();
            context.Items.Add(new Item { Name = "Coffee Machine", PriceInCents = 1000 });
            context.Items.Add(new Item { Name = "Coffee Grinder", PriceInCents = 250 });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.GetAsync("/Items/Details/1");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.Contains("Coffee Machine", html);
            Assert.DoesNotContain("Coffee Grinder", html);
            Assert.Contains("$10.00", html);
            Assert.DoesNotContain("$2.50", html);
        }

        [Fact]
        public async Task Test_Delete_OnlyDeletesOneItem()
        {
            var context = GetDbContext();
            Item CM = new Item { Name = "Coffee Machine", PriceInCents = 1000 };
            context.Items.Add(CM);
            context.Items.Add(new Item { Name = "Coffee Grinder", PriceInCents = 250 });
            context.SaveChanges();

            var client = _factory.CreateClient();
            var response = await client.PostAsync($"/Items/Details/Delete/{CM.Id}", null);
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain("Coffee Machine", html);
            Assert.Contains("Coffee Grinder", html);
        }

        [Fact]
        public async Task Edit_ReturnsFormViewPrePopulated()
        {
            // Arrange
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item vLatte = new Item { Name = "Vinilla Latte", PriceInCents = 450 };
            context.Items.Add(vLatte);
            context.SaveChanges();

            // Act
            var response = await client.GetAsync($"/Items/Details/{vLatte.Id}/Edit");
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.Contains("Edit Item", html);
            Assert.Contains(vLatte.Name, html);
            Assert.Contains(vLatte.PriceInCents.ToString(), html);
        }

        [Fact]
        public async Task Update_SavesChangesToMovie()
        {
            // Arrange
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item vLatte = new Item { Name = "Vinilla Latte", PriceInCents = 450 };
            context.Items.Add(vLatte);
            context.SaveChanges();

            var formData = new Dictionary<string, string>
            {
                { "Name", "Vinilla Latte" }
            };

            // Act
            var response = await client.PostAsync(
                $"/Items/Details/{vLatte.Id}",
                new FormUrlEncodedContent(formData)
            );
            var html = await response.Content.ReadAsStringAsync();

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Contains("Vinilla Latte", html);
        }
    }
}