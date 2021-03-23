using SehirRehberi.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Entities.Concrete
{
    public class City : IEntity
    {
        public int CityId { get; set; }

        //Foreign Keys
        public string UserId { get; set; }

        public string CityName { get; set; }
        public string CityDescription { get; set; }

        //Navigations
        public virtual ApplicationUser ApplicationUser { get; set; }
        public virtual ICollection<Photo> Photos { get; set; }
    }
}
