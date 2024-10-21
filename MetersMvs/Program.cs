using AutoMapper;
using Business;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using MetersMVC;
using Microsoft.EntityFrameworkCore;

namespace MetersMvs
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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


            var app = builder.Build();

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

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
