using Microsoft.AspNetCore.Identity;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Entities.Concrete
{
    public class ApplicationRoleClaim : IdentityRoleClaim<string>
    {
        public virtual ApplicationRole Role { get; set; }
    }
}
