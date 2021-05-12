using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Abstract
{
    public interface IAuthService
    {
        Task<IDataResult<TokensDto>> Login(UserForLoginDto userForLoginDto);
        Task<IResult> Register(UserForRegisterDto userForRegisterDto);
        Task<IResult> ChangePassword(UserForChangePasswordDto userForChangePasswordDto);
        Task<IDataResult<TokensDto>> Refresh(TokensDto model);
        Task<IDataResult<bool>> Validate(string token);
        Task<IResult> Revoke(string userName);
    }
}
