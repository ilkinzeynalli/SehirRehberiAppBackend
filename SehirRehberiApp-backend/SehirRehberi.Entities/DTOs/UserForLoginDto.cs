using SehirRehberi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Entities.DTOs
{
    public class UserForLoginDto: IDto
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
