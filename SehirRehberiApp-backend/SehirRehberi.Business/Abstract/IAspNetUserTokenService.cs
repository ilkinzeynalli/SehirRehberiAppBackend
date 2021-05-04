using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IAspNetUserTokenService
    {
        Task<IDataResult<string>> AddToken(ApplicationUserToken token);
        Task<IDataResult<List<ApplicationUserToken>>> GetTokensByUserId(string userId);
        Task<IResult> RemoveToken(ApplicationUserToken token);
        Task<IDataResult<ApplicationUserToken>> UpdateToken(ApplicationUserToken token);
    }
}
