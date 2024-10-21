using Business;
using Core.Interfaces;
using Infrastructure.Repositories;

namespace Meters
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
           services.AddScoped<IMeterRepository, MeterRepository>();
            services.AddScoped<IMeterService, MeterService>();
            services.AddScoped<IMeterReadingRepository, MeterReadingRepository>();
            services.AddScoped<IMeterReadingService, MeterReadingService>();



            return services;
        }
    }
}
