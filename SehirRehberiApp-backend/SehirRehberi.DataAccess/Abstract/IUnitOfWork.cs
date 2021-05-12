using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.DataAccess.Abstract
{
    public interface IUnitOfWork :IDisposable
    {
        IValueDal Values { get; }
        ICityDal Cities { get; }
        IPhotoDal Photos { get; }
        IAspNetUserTokenDal AspNetUserTokens { get; }

        Task<int> Complete();
    }
}
