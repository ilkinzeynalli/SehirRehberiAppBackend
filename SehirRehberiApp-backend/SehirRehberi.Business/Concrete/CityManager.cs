using AutoMapper;
using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Business.ValidationRules.FluentValidation;
using SehirRehberi.Core.Aspects.Autofac.Validation;
using SehirRehberi.Core.Utilities.Business;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Concrete
{
    public class CityManager : ICityService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public CityManager(IMapper mapper,IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(CityValidator))]
        public async Task<IDataResult<City>> AddCity(City city)
        {
            //Is kodlari
            var businessRulesResult = BusinessRules.Run((await CheckIfCityNameExist(city.CityName)),(await CheckIfCityCountOfUserCorrect(city.UserId)));

            if (businessRulesResult != null)
            {
                return new ErrorDataResult<City>(businessRulesResult.Message);
            }

            _unitOfWork.Cities.Add(city);
            await _unitOfWork.Complete();

            return new SuccessDataResult<City>(city, Messages.CityAdded);
        }

        public async Task<IDataResult<List<CityForListDto>>> GetAllCitiesWithPhotos()
        {
            //Is kodlari
            var businessRulesResult = BusinessRules.Run(CheckIfOutOfService());

            if (businessRulesResult != null)
            {
                return new ErrorDataResult<List<CityForListDto>> (businessRulesResult.Message);
            }

            var result = await _unitOfWork.Cities.GetAll().Include(i => i.Photos).ToListAsync();
            var resultForReturn = _mapper.Map<List<CityForListDto>>(result);

            return new SuccessDataResult<List<CityForListDto>> (resultForReturn, Messages.CityListed);
        }
        public async Task<IDataResult<CityForDetailDto>> GetCityById(int id)
        {
            var result = await _unitOfWork.Cities.GetEntityById(id);

            if (result != null)
            {
                var mappedData = _mapper.Map<CityForDetailDto>(result);

                return new SuccessDataResult<CityForDetailDto>(mappedData, Messages.CityDetail);
            }

            return new ErrorDataResult<CityForDetailDto>(Messages.CityNotFound);
        }

        public async Task<IDataResult<City>> GetCityWithPhotosById(int id)
        {
            var result = await _unitOfWork.Cities.GetAll(p => p.CityId == id).Include(i => i.Photos).FirstOrDefaultAsync();

            if (result != null)
                return new SuccessDataResult<City>(result, Messages.CityListed);

            return new ErrorDataResult<City>(Messages.CityNotFound);
        }

        private async Task<IResult> CheckIfCityNameExist(string cityName)
        {
            var result = await _unitOfWork.Cities.GetAll(c => c.CityName == cityName).AnyAsync();

            if (result)
            {
                return new ErrorResult(Messages.CityNameAlreadyExist);
            }

            return new SuccessResult();
        }

        private async Task<IResult> CheckIfCityCountOfUserCorrect(string userId)
        {
            var result = await _unitOfWork.Cities.GetAll(c => c.UserId == userId).CountAsync();

            if (result > 3)
                return new ErrorResult(Messages.CityCountOfUserError);

            return new SuccessResult();
        }

        private IResult CheckIfOutOfService()
        {
            if (DateTime.Now.Hour == 15)
            {
                return new ErrorResult(Messages.OutOfService);
            }

            return new SuccessResult();
        }
    }
}
