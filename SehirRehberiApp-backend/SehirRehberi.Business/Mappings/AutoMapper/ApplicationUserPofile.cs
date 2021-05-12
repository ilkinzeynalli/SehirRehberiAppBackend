using AutoMapper;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Mappings.AutoMapper
{
    public class ApplicationUserPofile : Profile
    {
        public ApplicationUserPofile()
        {
            CreateMap<UserForRegisterDto,ApplicationUser>();

        }
    }
}
