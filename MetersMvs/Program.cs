using AutoMapper;
using Business;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using Serilog;
using Core.Logging;

namespace MetersMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
 


            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });


            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");

            app.Run();
        }
    }
    }
