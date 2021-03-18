using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.DataAccess.Abstract
{
    public interface IUnitOfWork :IDisposable
    {
        IValueRepository Values { get; }
        ICityRepository Cities { get; }
        Task<int> Complete();
    }
}
