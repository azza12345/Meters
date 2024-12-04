using Core.Models;
using Infrastructure.Data;
using Infrastructure.Migrations;
using MetersMVC;
using MetersMVC.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace MetersIntegrationTests
{
    public class IntegrationTestAuth : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;
        private readonly MeterDbContext _context;
        //private readonly HttpClient _client;
        public IntegrationTestAuth(WebApplicationFactory<Program> factoryServer)
        {
            _factory = factoryServer;
            var scope = _factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<MeterDbContext>();

        }
       


        [Fact]
        public async Task Register_shouldAddUserSuccessfullyAndReturnOk()
        {
            //Arrange  
            var _client = _factory.CreateClient();
            RegisterViewModel registerModel = new RegisterViewModel()
            {
                
                FName ="Azza",
                LName ="Mohamed",
                Password = "Password@12345",
                Email = "test@test.com",
                Mobile="01234567890",
                Address="6th October"
            };


            //Act 
            var response = await _client.PostAsJsonAsync("https://localhost:7202/api/User/Register", registerModel);

            //Assert
            if (response.IsSuccessStatusCode)
            {
                var responseString = await response.Content.ReadAsStringAsync();
                Assert.Contains("User Created Successfully!", responseString);
            }
            else
            {
                Assert.Fail("Api call failed.");
            }

        }

        [Fact]
        public async Task Login_Should_Return_Ok_For_Authenticated_User()
        {
            //Arrange 
            var _client = _factory.CreateClient();
            LoginViewModel loginModel = new LoginViewModel()
            {
                Email = "test@test.com",
                Password = "Password@12345"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(loginModel),
                Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://localhost:7202/api/User/Login", content);
            response.EnsureSuccessStatusCode();


            var responseString = await response.Content.ReadAsStringAsync();

            var token = JsonConvert.DeserializeObject<TokenResponse>(responseString);

            Assert.NotNull(token);

         //   _context.Users.RemoveRange(_context.Users);
            _context.SaveChanges();

        }

        [Fact]
        public async Task Login_Should_Return_401_For_Authenticated_User()
        {
            //Arrange 
            var _client = _factory.CreateClient();
            LoginViewModel loginModel = new LoginViewModel()
            {
                Email = "test",
                Password = "Password"
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(loginModel),
                Encoding.UTF8, "application/json");

            var response = await _client.PostAsync("https://localhost:7202/api/User/Login", content);

            var result = response.IsSuccessStatusCode;
            Assert.False(result);

        }
    }
}
