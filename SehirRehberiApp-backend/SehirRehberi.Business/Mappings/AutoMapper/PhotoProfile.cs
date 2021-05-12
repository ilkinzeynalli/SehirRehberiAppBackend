using AutoMapper;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Mappings.AutoMapper
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<PhotoForCreationDto, Photo>()
                .ForMember(dest=>dest.PhotoUrl,option => option.MapFrom(src => src.Url) );

            CreateMap<Photo, PhotoForReturnDto>()
                .ForMember(dest => dest.Url, option => option.MapFrom(src => src.PhotoUrl)); ;
        }
    }
}
