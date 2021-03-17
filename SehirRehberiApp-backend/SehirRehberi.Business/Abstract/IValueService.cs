using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IValueService
    {
        Task<List<Value>> GetAllValues();
        Task<Value> GetValueById(int id);
        Task<Value> UpdateValue(Value value);
        Task<Value> AddValue(Value value);
        Task<int> DeleteValue(Value value);

    }
}
