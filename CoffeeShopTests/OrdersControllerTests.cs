using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTests
{
    [Collection("Controller Tests")]
    public class OrdersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        
        public OrdersControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task Index_ReturnsViewWithAllOrdersForAGivenCustomer()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            var customer2 = new Customer { Name = "James", Email = "james@james.james" };
            var order1 = new Order { DateCreated = new DateTime(2000, 2, 1).ToUniversalTime() };
            Item item1 = new Item { Name = "Coffee", PriceInCents = 299 };
            Item item2 = new Item { Name = "Donut", PriceInCents = 199 };
            order1.Items.Add(item1);
            order1.Items.Add(item2);
            customer1.Orders.Add(order1);
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();

            var response = await client.GetAsync("/customers/1/orders");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Items: 2", html);
            Assert.Contains(order1.DateCreated.ToString("mm-dd-yy"), html);
        }

        [Fact]
        public async Task Show_ReturnsViewWithInfoOnSingleOrder()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            var customer2 = new Customer { Name = "James", Email = "james@james.james" };
            var order1 = new Order { DateCreated = new DateTime(2000, 2, 1).ToUniversalTime() };
            Item item1 = new Item { Name = "Coffee", PriceInCents = 299 };
            Item item2 = new Item { Name = "Donut", PriceInCents = 199 };
            order1.Items.Add(item1);
            order1.Items.Add(item2);
            customer1.Orders.Add(order1);
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();

            var response = await client.GetAsync("/customers/1/orders/details/1");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Coffee", html);
            Assert.Contains("Donut", html);
            Assert.Contains("$4.98", html);
            Assert.Contains(order1.Id.ToString(), html);

        }

        [Fact]
        public async Task New_DisplaysFormWithDateCreated()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            var order1 = new Order { DateCreated = new DateTime(2000, 2, 1).ToUniversalTime() };
            customer1.Orders.Add(order1);
            context.Customers.Add(customer1);
            context.SaveChanges();

            context.SaveChanges();

            var response = await client.GetAsync($"/customers/1/orders/new");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("<form method=\"post\" action=\"/customers/1/orders\">", html);
            Assert.Contains("<input type=\"date\" id=\"DateCreated\" name=\"DateCreated\" required />", html);
        }

        [Fact]
        public async Task Create_AddsOrderToDB()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();
            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            context.Customers.Add(customer1);
            context.SaveChanges();

            var formData = new Dictionary<string, string>
            {
                {"DateCreated","01-01-02" }
            };


            var response = await client.PostAsync($"/customers/{customer1.Id}/orders", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("00-01-02", html);
        }
    }
}