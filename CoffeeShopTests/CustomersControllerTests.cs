using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTests
{
    public class CustomersControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        
        public CustomersControllerTests(WebApplicationFactory<Program> factory)
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
        public async Task Index_ShowsListOfAllCustomers()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            var customer2 = new Customer { Name = "James", Email = "james@james.james" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();

            var response = await client.GetAsync("/customers");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains(customer1.Name, html);
            Assert.Contains(customer2.Name, html);
            Assert.DoesNotContain(customer1.Email, html);
            Assert.DoesNotContain(customer2.Email, html);
        }
    }
}