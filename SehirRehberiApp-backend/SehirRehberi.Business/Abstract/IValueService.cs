using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IValueService
    {
        Task<IDataResult<List<Value>>> GetAllValues();
        Task<IDataResult<Value>> GetValueById(int id);
        Task<IDataResult<Value>> UpdateValue(Value value);
        Task<IResult> AddValue(Value value);
        Task<IResult> DeleteValue(int id);

    }
}
