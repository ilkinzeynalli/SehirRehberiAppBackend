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
    public class AspNetUserTokenManager : IAspNetUserTokenService
    {
        private readonly IUnitOfWork _unitOfWork;
        public AspNetUserTokenManager(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IDataResult<string>> AddToken(ApplicationUserToken token)
        {

            _unitOfWork.AspNetUserTokens.Add(token);
            await _unitOfWork.Complete();

            return new SuccessDataResult<string>(token.Value,Messages.TokenAdded);

        }

        public async Task<IDataResult<List<ApplicationUserToken>>> GetTokensByUserId(string userId)
        {

            var result = await _unitOfWork.AspNetUserTokens.GetAll(p => p.UserId == userId).ToListAsync();

            if (result == null)
                return new ErrorDataResult<List<ApplicationUserToken>>(Messages.TokensNotFound);

            return new SuccessDataResult<List<ApplicationUserToken>>(result);
        }

        public async Task<IResult> RemoveToken(ApplicationUserToken token)
        {

            _unitOfWork.AspNetUserTokens.Delete(token);
            await _unitOfWork.Complete();

            return new SuccessResult(Messages.TokenRemoved);

        }
        public async Task<IDataResult<ApplicationUserToken>> UpdateToken(ApplicationUserToken token)
        {

            _unitOfWork.AspNetUserTokens.Update(token);
            await _unitOfWork.Complete();

            return new SuccessDataResult<ApplicationUserToken>(token,Messages.TokenUpdated);

        }
    }
}
