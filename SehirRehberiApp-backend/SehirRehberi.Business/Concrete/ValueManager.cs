using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Business.ValidationRules.FluentValidation;
using SehirRehberi.Core.Aspects.Autofac.Validation;
using SehirRehberi.Core.Utilities.Business;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Concrete
{
    public class ValueManager : IValueService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ValueManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IDataResult<List<Value>>> GetAllValues()
        {
            return new SuccessDataResult<List<Value>>(await _unitOfWork.Values.GetAll().ToListAsync(),Messages.ValueListed);
        }
        public async Task<IDataResult<Value>> GetValueById(int id)
        {
            var result = await _unitOfWork.Values.GetEntityById(id);

            if (result == null)
                return new ErrorDataResult<Value>(Messages.ValueNotFound);

            return new SuccessDataResult<Value>(result);
        }

        [ValidationAspect(typeof(ValueValidator))]
        public async Task<IResult> AddValue(Value model)
        {
            var result = BusinessRules.Run((await CheckIfValueCountOfCorrect()));

            if (result != null)
                return new ErrorResult(result.Message);

            _unitOfWork.Values.Add(model);
            await _unitOfWork.Complete();

            return new SuccessResult(Messages.ValueAdded);
        }

        [ValidationAspect(typeof(ValueValidator))]
        public async Task<IDataResult<Value>> UpdateValue(Value model)
        {
            _unitOfWork.Values.Update(model);
            await _unitOfWork.Complete();

            return new SuccessDataResult<Value>(model,Messages.ValueUpdated);
        }

        public async Task<IResult> DeleteValue(int id)
        {
            var selectedValue = await GetValueById(id);

            if (!selectedValue.Success)
                return new ErrorResult(selectedValue.Message);

            _unitOfWork.Values.Delete(selectedValue.Data);
            await _unitOfWork.Complete();

            return new SuccessResult(Messages.ValueDeleted);
        }

        private async Task<IResult> CheckIfValueCountOfCorrect()
        {
           var countOfValue = await _unitOfWork.Values.GetAll().CountAsync();

            if (countOfValue > 5)
                return new ErrorResult(Messages.ValueCountOfCorrectInvalid);

            return new SuccessResult();
        }
    }
}
