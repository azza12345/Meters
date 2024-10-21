using AutoMapper;
using Core.Models;
using Infrastructure.Entities;
using Meters.ViewModels;

namespace Meters
{
    public class MappingProfile :Profile
    {
        public MappingProfile()
        {
            
            CreateMap<MeterEntity, Meter>().ReverseMap();
            CreateMap<MeterReadingEntity, MeterReading>().ReverseMap();

           
            CreateMap<Meter, MeterViewModel>().ReverseMap();
            CreateMap<MeterReading, MeterReadingViewModel>().ReverseMap();
        }
    }
}

