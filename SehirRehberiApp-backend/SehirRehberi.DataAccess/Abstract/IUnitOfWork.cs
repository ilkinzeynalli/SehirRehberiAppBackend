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
        IPhotoRepository Photos { get; }
        IAspNetUserToken AspNetUserTokens { get; }

        Task<int> Complete();
    }
}
