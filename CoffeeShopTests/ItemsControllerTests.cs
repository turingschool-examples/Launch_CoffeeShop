using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using System.Runtime.CompilerServices;
 

namespace CoffeeShopTests
{

    [Collection("Items Controller Tests")]
    public class CoffeeShopMVCItemsTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CoffeeShopMVCItemsTests(WebApplicationFactory<Program> factory)
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

            response.EnsureSuccessStatusCode();
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

            response.EnsureSuccessStatusCode();
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

            response.EnsureSuccessStatusCode();

            Assert.Contains("Dirt", html);
            Assert.Contains("1", html);
        }

        [Fact]
        public async Task Details_ShowsItemDetails()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item dirt = new Item { Name = "Dirt", PriceInCents = 100 };
            context.Add(dirt);
            context.SaveChanges();

            var response = await client.GetAsync($"/items/details/{dirt.Id}");
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains("Dirt", html);
            Assert.Contains("1", html);
        }

        [Fact]
        public async Task Delete_DeletesItemFromDB()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item dirt = new Item { Name = "Dirt", PriceInCents = 100 };
            context.Add(dirt);
            context.SaveChanges();

            var response = await client.PostAsync($"/items/delete/{dirt.Id}", null);
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain("Dirt", html);
        }

        [Fact]

        public async Task Edit_ReturnsEditForm()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item dirt = new Item { Name = "Dirt", PriceInCents = 100 };
            context.Add(dirt);
            context.SaveChanges();

            var response = await client.PostAsync($"/items/edit/{dirt.Id}", null);
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains("Dirt", html);
            Assert.Contains("<form method=\"post\" action=\"/items/1\">", html);
            Assert.Contains("<button type=\"submit\">Save Changes</button>", html);
            Assert.Contains("<button type=\"submit\">Go Back to Items List</button>", html);

        }

        [Fact]
        public async Task Update_SavesItemChanges()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Item dirt = new Item { Name = "Dirt", PriceInCents = 100 };
            context.Add(dirt);
            context.SaveChanges();

            var addItemFormData = new Dictionary<string, string>
            {
                {"Name", "Sand" },
                {"PriceInCents", "120" }
            };

            var response = await client.PostAsync($"/items/{dirt.Id}", new FormUrlEncodedContent(addItemFormData));
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains("Sand", html);
            Assert.Contains("1.2", html);
            Assert.DoesNotContain("Dirt", html);
            Assert.DoesNotContain("1.00", html);
        }

    }
}