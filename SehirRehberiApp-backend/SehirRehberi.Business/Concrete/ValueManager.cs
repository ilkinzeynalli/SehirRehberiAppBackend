using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Concrete
{
    public class ValueManager : IValueService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<ValueManager> _logger;

        public ValueManager(IUnitOfWork unitOfWork, ILogger<ValueManager> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        public async Task<IDataResult<List<Value>>> GetAllValues()
        {
            try
            {
                return new SuccessDataResult<List<Value>>(await _unitOfWork.Values.GetAll().ToListAsync());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }
        public async Task<IDataResult<Value>> GetValueById(int id)
        {
            try
            {
                return new SuccessDataResult<Value>(await _unitOfWork.Values.GetEntityById(id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
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
            try
            {
                _unitOfWork.Values.Update(model);
                await _unitOfWork.Complete();

                return new SuccessDataResult<Value>(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }
        public async Task<IDataResult<int>> DeleteValue(Value model)
        {
            try
            {
                _unitOfWork.Values.Delete(model);
                return new SuccessDataResult<int>(await _unitOfWork.Complete());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }

    }
}
