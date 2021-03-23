using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface ICityService
    {
        Task<List<City>> GetAllCitiesWithPhotos();
        Task<City> AddCity(City city);
        Task<City> GetCityById(int id);
    }
}
