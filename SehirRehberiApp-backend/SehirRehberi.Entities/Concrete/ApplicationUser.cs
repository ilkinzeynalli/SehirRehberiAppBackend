using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Entities.Concrete
{
    public class ApplicationUser : IdentityUser
    {
        public ICollection<City> Cities { get; set; }
    }
}
