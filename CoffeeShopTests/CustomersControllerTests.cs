using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using CoffeeShopMVC.Models;
using CoffeeShopMVC.DataAccess;
using CoffeeShopMVC.FeatureTests;
using System.Runtime.CompilerServices;

namespace CoffeeShopTests
{
    [Collection("Customers Controller Tests")]
    public class CoffeeShopMVCCustomersTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public CoffeeShopMVCCustomersTests(WebApplicationFactory<Program> factory)
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
        public async Task Edit_ReturnsFormToEdit()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Customer customer = new Customer { Name = "john doe", Email = "johndoe123@gmail.com"};
            context.Add(customer);
            context.SaveChanges();

            var response = await client.PostAsync($"/customers/edit/{customer.Id}", null);
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains("john doe", html);
            Assert.Contains("johndoe123@gmail.com", html);
            Assert.Contains("<form method=\"post\" action=\"/customers/1\">", html);
            Assert.Contains("<button type=\"submit\">Save Changes</button>", html);
        }

        [Fact]
        public async Task Update_SavesNewCustomerInformation()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Customer customer = new Customer { Name = "john", Email = "johndoe123@gmail.com" };
            context.Add(customer);
            context.SaveChanges();

            var addItemFormData = new Dictionary<string, string>
            {
                {"Name", "Mrs. Puff" },
                {"Email", "teachpuff600@gmail.com" }
            };

            var response = await client.PostAsync($"/customers/{customer.Id}", new FormUrlEncodedContent(addItemFormData));
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();

            Assert.Contains("Mrs. Puff", html);
            Assert.Contains("teachpuff600@gmail.com", html);
            Assert.DoesNotContain("john doe", html);
            Assert.DoesNotContain("johndoe123@gmail.com", html);
        }

        [Fact]
        public async Task Delete_RemovesCustomer()
        {
            var context = GetDbContext();
            var client = _factory.CreateClient();

            Customer customer = new Customer { Name = "john doe", Email = "johndoe123@gmail.com" };
            context.Add(customer);
            context.SaveChanges();

            var response = await client.PostAsync($"/customers/delete/{customer.Id}", null);
            var html = await response.Content.ReadAsStringAsync();

            response.EnsureSuccessStatusCode();
            Assert.DoesNotContain("john doe", html);
        }
    }
}
