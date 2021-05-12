using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
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
        public async Task<IResult> AddValue(Value model)
        {
            if (model == null)
                return new ErrorResult(Messages.ValueNotAdded);

            _unitOfWork.Values.Add(model);
            await _unitOfWork.Complete();

            return new SuccessResult(Messages.ValueAdded);
        }
        public async Task<IDataResult<Value>> UpdateValue(Value model)
        {
            _unitOfWork.Values.Update(model);
            await _unitOfWork.Complete();

            return new SuccessDataResult<Value>(model,Messages.ValueUpdated);
        }
        public async Task<IResult> DeleteValue(Value model)
        {
            _unitOfWork.Values.Delete(model);
            await _unitOfWork.Complete();

            return new SuccessResult(Messages.ValueDeleted);
        }

    }
}
