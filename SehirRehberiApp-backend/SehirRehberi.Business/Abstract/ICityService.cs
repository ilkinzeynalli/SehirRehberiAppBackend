using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface ICityService
    {
        Task<IDataResult<List<City>>> GetAllCitiesWithPhotos();
        Task<IDataResult<City>> AddCity(City city);
        Task<IDataResult<City>> GetCityById(int id);
        Task<IDataResult<City>> GetCityWithPhotosById(int id);

    }
}
