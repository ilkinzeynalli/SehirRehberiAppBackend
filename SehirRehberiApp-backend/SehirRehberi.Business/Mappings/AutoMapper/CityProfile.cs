using AutoMapper;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Mappings.AutoMapper
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<City, CityForListDto>()
                .ForMember(dest => dest.Id, opt =>
                {
                    opt.MapFrom(src => src.CityId);
                })
                .ForMember(dest => dest.Name, opt =>
                {
                    opt.MapFrom(src => src.CityName);
                })
                .ForMember(dest => dest.PhotoUrl, opt =>
                {
                    opt.MapFrom(src => src.Photos.FirstOrDefault(f => f.IsMain).PhotoUrl);
                })
                .ForMember(dest => dest.Description, opt =>
                {
                    opt.MapFrom(src => src.CityDescription);
                });

            CreateMap<City, CityForDetailDto>();
        }
    }
}
