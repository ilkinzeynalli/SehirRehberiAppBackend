using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Concrete
{
    public class CityManager : ICityService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CityManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<City> AddCity(City city)
        {
            _unitOfWork.Cities.Add(city);
            await _unitOfWork.Complete();

            return city;
        }

        public async Task<List<City>> GetAllCitiesWithPhotos()
        {
            var result = await _unitOfWork.Cities.GetAll().Include(i=>i.Photos).ToListAsync();

            return result;
        }
        public async Task<City> GetCityById(int id)
        {
           var result =  await _unitOfWork.Cities.GetEntityById(id);

            return result;
        }

        public async Task<City> GetCityWithPhotosById(int id)
        {
            var result = await _unitOfWork.Cities.GetAll(p=>p.CityId == id).Include(i => i.Photos).FirstOrDefaultAsync();

            return result;
        }
    }
}
