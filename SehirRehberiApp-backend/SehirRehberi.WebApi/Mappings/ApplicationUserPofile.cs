using AutoMapper;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.Mappings
{
    public class ApplicationUserPofile : Profile
    {
        public ApplicationUserPofile()
        {
            CreateMap<UserForRegisterDto,ApplicationUser>();
        }
    }
}
