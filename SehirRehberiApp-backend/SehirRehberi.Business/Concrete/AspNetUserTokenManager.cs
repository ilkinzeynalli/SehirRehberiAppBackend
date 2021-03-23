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
    public class AspNetUserTokenManager : IAspNetUserTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AspNetUserTokenManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApplicationUserToken> AddToken(ApplicationUserToken token)
        {
            _unitOfWork.AspNetUserTokens.Add(token);
            await _unitOfWork.Complete();

            return token;
        }

        public async Task<List<ApplicationUserToken>> GetTokensByUserId(string userId)
        {
            var result = await _unitOfWork.AspNetUserTokens.GetAll(p=>p.UserId == userId).ToListAsync();
            return result;
        }

        public async Task RemoveToken(ApplicationUserToken token)
        {
            _unitOfWork.AspNetUserTokens.Delete(token);
            await _unitOfWork.Complete();
        }

        public async Task<ApplicationUserToken> UpdateToken(ApplicationUserToken token)
        {
            _unitOfWork.AspNetUserTokens.Update(token);
            await _unitOfWork.Complete();

            return token;
        }
    }
}
