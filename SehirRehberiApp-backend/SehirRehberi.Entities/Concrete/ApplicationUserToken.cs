using Microsoft.AspNetCore.Identity;
using SehirRehberi.Core.Entities;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Entities.Concrete
{ 
    public class ApplicationUserToken :IdentityUserToken<string>,IEntity
    {
        public DateTime ExpireDate { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
