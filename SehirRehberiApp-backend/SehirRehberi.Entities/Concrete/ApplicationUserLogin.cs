using Microsoft.AspNetCore.Identity;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Entities.Concrete
{
    public class ApplicationUserLogin : IdentityUserLogin<string>
    {
        public virtual ApplicationUser User { get; set; }
    }
}
