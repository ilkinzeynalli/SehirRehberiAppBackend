using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SehirRehberi.Business.Abstract;
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
        private readonly ILogger<AspNetUserTokenManager> _logger;
        public AspNetUserTokenManager(IUnitOfWork unitOfWork, ILogger<AspNetUserTokenManager> logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }
        public async Task<IDataResult<ApplicationUserToken>> AddToken(ApplicationUserToken token)
        {
            try
            {
                _unitOfWork.AspNetUserTokens.Add(token);
                await _unitOfWork.Complete();

                return new SuccessDataResult<ApplicationUserToken>(token);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }

        }

        public async Task<IDataResult<List<ApplicationUserToken>>> GetTokensByUserId(string userId)
        {
            try
            {
                var result = await _unitOfWork.AspNetUserTokens.GetAll(p => p.UserId == userId).ToListAsync();

                return new SuccessDataResult<List<ApplicationUserToken>>(result);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
            
        }

        public async Task<IResult> RemoveToken(ApplicationUserToken token)
        {
            try
            {
                _unitOfWork.AspNetUserTokens.Delete(token);
                await _unitOfWork.Complete();

                return new SuccessResult();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }
        public async Task<IDataResult<ApplicationUserToken>> UpdateToken(ApplicationUserToken token)
        {
            try
            {
                _unitOfWork.AspNetUserTokens.Update(token);
                await _unitOfWork.Complete();

                return new SuccessDataResult<ApplicationUserToken>(token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message ?? ex.InnerException.Message);
                throw ex;
            }
        }
    }
}
