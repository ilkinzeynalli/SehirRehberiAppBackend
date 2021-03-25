using AutoMapper;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.WebApi.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.Mappings
{
    public class PhotoProfile : Profile
    {
        public PhotoProfile()
        {
            CreateMap<PhotoForCreationDTO, Photo>()
                .ForMember(dest=>dest.PhotoUrl,option => option.MapFrom(src => src.Url) );

            CreateMap<Photo, PhotoForReturnDTO>()
                .ForMember(dest => dest.Url, option => option.MapFrom(src => src.PhotoUrl)); ;
        }
    }
}
