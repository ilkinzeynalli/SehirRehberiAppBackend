using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface ICityService
    {
        Task<IDataResult<List<CityForListDto>>> GetAllCitiesWithPhotos();
        Task<IDataResult<City>> AddCity(City city);
        Task<IDataResult<CityForDetailDto>> GetCityById(int id);
        Task<IDataResult<City>> GetCityWithPhotosById(int id);

    }
}
