using Core.Models;
using AutoMapper;
using MetersMVC.ViewModels;
using Infrastructure.Entities;

namespace MetersMVC
{
    public class MappingProfiles :Profile
    {
        public MappingProfiles()
        {
            CreateMap<MeterEntity, Meter>().ReverseMap();
            CreateMap<MeterReadingEntity, MeterReading>().ReverseMap();

            
            CreateMap<Meter, MeterViewModel>().ReverseMap();
            CreateMap<MeterReading, MeterReadingViewModel>().ReverseMap();


        }
    }
}
