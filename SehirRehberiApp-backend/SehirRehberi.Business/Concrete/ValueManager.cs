using Microsoft.EntityFrameworkCore;
using SehirRehberi.Business.Abstract;
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

        public ValueManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<List<Value>> GetAllValues()
        {
            return await _unitOfWork.Values.GetAll().ToListAsync();
        }
        public async Task<Value> GetValueById(int id)
        {
            return await _unitOfWork.Values.GetEntityById(id);
        }
        public async Task<Value> AddValue(Value model)
        {
            _unitOfWork.Values.Add(model);
            await _unitOfWork.Complete();

            return model;
        }
        public async Task<Value> UpdateValue(Value model)
        {
            _unitOfWork.Values.Update(model);
            await _unitOfWork.Complete();

            return model;
        }
        public async Task<int> DeleteValue(Value model)
        {
            _unitOfWork.Values.Delete(model);
            return await _unitOfWork.Complete();
        }
      
    }
}
