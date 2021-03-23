using SehirRehberi.Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IAspNetUserTokenService
    {
        Task<ApplicationUserToken> AddToken(ApplicationUserToken token);
        Task<List<ApplicationUserToken>> GetTokensByUserId(string userId);
        Task RemoveToken(ApplicationUserToken token);
        Task<ApplicationUserToken> UpdateToken(ApplicationUserToken token);
    }
}
