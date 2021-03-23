using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.Dtos
{
    public class CityForDetailDTO
    {
        public int CityId { get; set; }
        public string CityName { get; set; }
        public string CityDescription { get; set; }
        public List<Photo> Photos { get; set; }
    }
}
