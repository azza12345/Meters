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
            
            
            CreateMap<Meter, MeterViewModel>().ReverseMap();
            
            CreateMap<MeterEntity, MeterViewModel>().ReverseMap();
           
            CreateMap<MeterReadingEntity, MeterReading>()
          .ForMember(dest => dest.Meter, opt => opt.Ignore()) 
          .ReverseMap()
          .ForMember(dest => dest.Meter, opt => opt.Ignore());

            CreateMap<MeterReadingViewModel, MeterReading>().ReverseMap();


            CreateMap<MeterEntity, Meter>()
           .ReverseMap();

          

        }
    }
}
