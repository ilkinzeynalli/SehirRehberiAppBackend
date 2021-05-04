using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Utilities.Results;
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

        public async Task<IDataResult<City>> AddCity(City city)
        {

            _unitOfWork.Cities.Add(city);
            await _unitOfWork.Complete();

            return new SuccessDataResult<City>(city,Messages.CityAdded);
        }

        public async Task<IDataResult<List<City>>> GetAllCitiesWithPhotos()
        {
            var result = await _unitOfWork.Cities.GetAll().Include(i => i.Photos).ToListAsync();

            return new SuccessDataResult<List<City>>(result,Messages.CityListed);
        }
        public async Task<IDataResult<City>> GetCityById(int id)
        {
            var result = await _unitOfWork.Cities.GetEntityById(id);

            if (result != null)
                return new SuccessDataResult<City>(result, Messages.CityDetail);

            return new ErrorDataResult<City>(Messages.CityNotFound);
        }

        public async Task<IDataResult<City>> GetCityWithPhotosById(int id)
        {
            var result = await _unitOfWork.Cities.GetAll(p => p.CityId == id).Include(i => i.Photos).FirstOrDefaultAsync();

            if (result != null)
                return new SuccessDataResult<City>(result, Messages.CityListed);

            return new ErrorDataResult<City>(Messages.CityNotFound);
        }
    }
}
