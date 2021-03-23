using SehirRehberi.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace SehirRehberi.Entities.Concrete
{
    public class Photo : IEntity
    {
        public int PhotoId { get; set; }

        //Foreign Keys
        public int CityId { get; set; }

        public DateTime DateAdded { get; set; }
        public string Description { get; set; }
        public bool IsMain { get; set; }
        public string PublicId { get; set; }
        public string PhotoUrl { get; set; }

        //Navigations
        public virtual City City { get; set; }
    }
}
