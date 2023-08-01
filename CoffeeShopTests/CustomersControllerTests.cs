using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;

namespace CoffeeShopTests
{
    [Collection("Controller Tests")]
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

        [Fact]
        public async Task New_DisplaysFormWithNameAndEmail()
        {
            var client = _factory.CreateClient();


            var response = await client.GetAsync("/customers/new");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("<form method=\"post\" action=\"/customers/create\">", html);
            Assert.Contains("<input type=\"text\" id=\"Name\" name=\"Name\" required />", html);
            Assert.Contains("<input type=\"email\" id=\"Email\" name=\"Email\" required />", html);
        }

        [Fact]
        public async Task Create_AddsCustomerToDatabase()
        {
            var client = _factory.CreateClient();
            var formData = new Dictionary<string, string>
            {
                {"Name","Eli" },
                {"Email","Eli@gmail" }
            };

            var response = await client.PostAsync($"/customers/create", new FormUrlEncodedContent(formData));
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Eli", html);
        }

        [Fact]
        public async Task Show_ShowsViewWithOnlyOneCustomer()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            var customer2 = new Customer { Name = "James", Email = "james@james.james" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();

            var response = await client.GetAsync($"/customers/details/{customer1.Id}");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("John", html);
            Assert.DoesNotContain("James", html);
            Assert.Contains("$0", html);
        }
        [Fact]
        public async Task Edit_ReturnsViewWithPrePopulatedForm()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            var customer2 = new Customer { Name = "James", Email = "james@james.james" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();
            var response = await client.GetAsync($"/customers/edit/{customer1.Id}");
            response.EnsureSuccessStatusCode();

            var html = await response.Content.ReadAsStringAsync();
            Assert.Contains($"<form method=\"post\" action=\"/customers/edit/{customer1.Id}\">", html);
            Assert.Contains(customer1.Name, html);
            Assert.Contains(customer1.Email, html);
        }
        [Fact]
        public async Task Update_RedirectsToShowPageWithUpdatedInfo()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            var customer1 = new Customer { Name = "John", Email = "john@john.john" };
            var customer2 = new Customer { Name = "James", Email = "james@james.james" };
            context.Customers.Add(customer1);
            context.Customers.Add(customer2);
            context.SaveChanges();

            var formData = new Dictionary<string, string>
            {
                {"Name","Eli" },
                {"Email","Eli@gmail" }
            };

            var response = await client.PostAsync($"/customers/edit/{customer1.Id}", new FormUrlEncodedContent(formData));
            response.EnsureSuccessStatusCode();
            var html = await response.Content.ReadAsStringAsync();

            Assert.Contains("Eli", html);
            Assert.DoesNotContain("John", html);
        }
    }
}