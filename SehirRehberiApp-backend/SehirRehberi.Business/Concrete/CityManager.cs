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
        private readonly ILogger<CityManager> _logger;

        public CityManager(IUnitOfWork unitOfWork, ILogger<CityManager> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IDataResult<City>> AddCity(City city)
        {
            try
            {
                _unitOfWork.Cities.Add(city);
                await _unitOfWork.Complete();

                return new SuccessDataResult<City>(city);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }

        public async Task<IDataResult<List<City>>> GetAllCitiesWithPhotos()
        {
            try
            {
                var result = await _unitOfWork.Cities.GetAll().Include(i => i.Photos).ToListAsync();

                return new SuccessDataResult<List<City>>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }
        public async Task<IDataResult<City>> GetCityById(int id)
        {
            try
            {
                var result = await _unitOfWork.Cities.GetEntityById(id);

                return new SuccessDataResult<City>(result,Messages.CityDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }

        public async Task<IDataResult<City>> GetCityWithPhotosById(int id)
        {
            try
            {
                var result = await _unitOfWork.Cities.GetAll(p => p.CityId == id).Include(i => i.Photos).FirstOrDefaultAsync();

                return new SuccessDataResult<City>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }
    }
}
