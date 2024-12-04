using AutoMapper;
using Business;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MetersMVC;
using Microsoft.EntityFrameworkCore;
using System;
using Serilog;
using Core.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;


namespace MetersMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            builder.Services.AddControllersWithViews();

          

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            builder.Services.AddAuthentication(options =>
            {
               // options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
 .AddJwtBearer(options =>
 {
     var key = builder.Configuration["Jwt:Key"];
     if (string.IsNullOrEmpty(key))
     {
         throw new ArgumentNullException("Jwt:Key", "JWT key is not configured.");
     }

     options.TokenValidationParameters = new TokenValidationParameters
     {
         ValidateIssuer = true,
         ValidateAudience = true,
         ValidateLifetime = true,
         ValidIssuer = builder.Configuration["Jwt:Issuer"],
         ValidAudience = builder.Configuration["Jwt:Audience"],
         IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
     };
 });


           //builder.Services.AddDbContext<MeterDbContext>(options =>
           // options.UseInMemoryDatabase("TestDb"));


            builder.Services.AddHttpContextAccessor();


            Log.Logger = new LoggerConfiguration()
              .ReadFrom.Configuration(builder.Configuration)
              .CreateLogger();

            builder.Host.UseSerilog();

         

         //  Log.Information("Hello, {Name}!", Environment.UserName);


              

            builder.Services.AddDbContext<MeterDbContext>(options =>
               options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));


            var mappingProfile = new MapperConfiguration(m =>
            {
                m.AddProfile(new MappingProfiles());
            });

            

            IMapper mapper = mappingProfile.CreateMapper();
            builder.Services.AddSingleton(mapper);

            builder.Services.AddScoped<IMeterRepository, MeterRepository>();
            builder.Services.AddScoped<IMeterService, MeterService>();
            builder.Services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
            builder.Services.AddScoped<IMeterReadingService, MeterReadingService>();


            builder.Services.AddHttpClient("MetersApiClient", client =>
            {
                client.BaseAddress = new Uri("https://localhost:7182/api/");
            });





            builder.Services.AddSingleton<Core.Logging.ILogger>(provider =>
          new Logger(LoggingDestination.Seq));

            builder.Services.AddHttpClient();

          

            builder.Services.AddHttpContextAccessor(); 
           // builder.Services.AddControllersWithViews();


            builder.Services.AddDistributedMemoryCache();

            // builder.Services.AddControllersWithViews(options =>
            // {
            //    options.Filters.Add<RequireAuthorizationAttribute>(); 
            //});
            //  builder.Services.AddSession();

            builder.Services.AddDistributedMemoryCache();  
            builder.Services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true; 
            });

            builder.Services.AddControllersWithViews(options =>
            {
             //   options.Filters.Add<RequireAuthorizationAttribute>(); 
            });


            builder.Services.AddControllers();
            builder.Services.AddDbContext<MeterDbContext>(options =>
                options.UseInMemoryDatabase("TestDb"));
            builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                .AddEntityFrameworkStores<MeterDbContext>()
                .AddDefaultTokenProviders();
            builder.Services.AddDistributedMemoryCache();

            var app = builder.Build();

           

            // Initialize LoggerHelper with the custom Logger instance
            var logger = app.Services.GetRequiredService<Core.Logging.ILogger>();
            LoggerHelper.Initialize(logger);

          


            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection(); // Redirects to HTTPS early in the pipeline
            app.UseStaticFiles();       // Serves static files

            app.UseRouting();           // Sets up routing

            app.UseSession();           // Enables session before authentication and authorization
            app.UseAuthentication();    // Ensures authentication middleware can access session if needed
            app.UseAuthorization();     // Applies authorization rules

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); // Maps controllers
            });





            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
