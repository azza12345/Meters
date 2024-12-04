using Infrastructure.Data;
using MetersIntegrationTests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Writers;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MetersIntegrationTests
{
    internal class TestServer<TProgram> : WebApplicationFactory<TProgram> where TProgram : Program
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                // Use in-memory database
                services.RemoveAll(typeof(DbContextOptions<MeterDbContext>));
                services.AddDbContext<MeterDbContext>(options =>
                {
                    options.UseInMemoryDatabase("InMemoryDatabaseForTesting");
                });

                // Configure identity
                services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<MeterDbContext>()
                    .AddDefaultTokenProviders();

                // Authentication for test cases
                services.AddAuthentication("testScheme").AddScheme<AuthenticationSchemeOptions, TestAuthHandler>
                ("testScheme", _ => { });
            });

            builder.Configure(app =>
            {
                app.UseRouting();
                app.UseAuthentication();
                app.UseAuthorization();
                app.UseEndpoints(endpoints => endpoints.MapControllers());
            });
        }


        /*private static string? GetConnectionString()
        {
            var config = new ConfigurationBuilder().AddUserSecrets<WebApplicationFactory<TStartup>>().Build();
            var connString = "Server=.\\SQLEXPRESS;Database=TestNewAuthWebAPI;Trusted_Connection=True;TrustServerCertificate=True;";
            return connString;
        }

        private static AppDbContext GetDbContext(IServiceCollection services)
        {
            var serviceProvider = services.BuildServiceProvider();
            var scope = serviceProvider.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            return dbContext;
        }*/
    }
}