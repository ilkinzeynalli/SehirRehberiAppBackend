using AutoMapper;
using Microsoft.AspNetCore.Identity;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Business.ValidationRules.FluentValidation;
using SehirRehberi.Core.Aspects.Autofac.Validation;
using SehirRehberi.Core.Extensions;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Core.Utilities.Security.Jwt;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SehirRehberi.Business.Concrete
{
    public class AuthManager : IAuthService
    {

        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ITokenHelper _tokenHelper;
        private readonly IAspNetUserTokenService _aspNetUserTokenManager;
        public AuthManager(IMapper mapper,
                             UserManager<ApplicationUser> userManager,
                             RoleManager<ApplicationRole> roleManager,
                             SignInManager<ApplicationUser> signInManager,
                             ITokenHelper tokenHelper,
                             IAspNetUserTokenService aspNetUserTokenManager)
        {
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            _tokenHelper = tokenHelper;
            _aspNetUserTokenManager = aspNetUserTokenManager;

        }
       
        [ValidationAspect(typeof(UserForLoginDtoValidator))]
        public async Task<IDataResult<TokensDto>> Login(UserForLoginDto userForLoginDto)
        {
            var user = await _userManager.FindByNameAsync(userForLoginDto.UserName);

            if (user != null)
            {
                await _signInManager.SignOutAsync();

                var signInResult = await _signInManager.PasswordSignInAsync(user, userForLoginDto.Password, false, false);

                if (signInResult.Succeeded)
                {
                    await _userManager.ResetAccessFailedCountAsync(user);

                    var userRoles = await _userManager.GetRolesAsync(user);

                    var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier,user.Id),
                            new Claim(ClaimTypes.Name, user.UserName),
                            new Claim("userId", user.Id),
                            new Claim("userName", user.UserName),
                            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        };

                    foreach (var userRole in userRoles)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, userRole));
                    }

                    #region Token Check and Assign In DB

                    var existTokens = await _aspNetUserTokenManager.GetTokensByUserId(user.Id);
                    if (existTokens.Success && existTokens.Data.Count > 0)
                    {
                        foreach (var removedToken in existTokens.Data)
                            await _aspNetUserTokenManager.RemoveToken(removedToken);
                    }

                    var accessToken = _tokenHelper.GenerateAccessToken(claims);
                    var accessTokeExpireDate = new JwtSecurityToken(accessToken).ValidTo.ConvertUtcToLocalTime();
                    var refreshToken = _tokenHelper.GenerateRefreshToken();
                    var refreshTokenExpireDate = accessTokeExpireDate.AddMinutes(2);

                    var resultForAccessToken = await _aspNetUserTokenManager.AddToken(new ApplicationUserToken() { UserId = user.Id, LoginProvider = "MyApp", Name = TokenTypes.AccessToken, Value = accessToken, ExpireDate = accessTokeExpireDate });
                    var resultForRefreshToken = await _aspNetUserTokenManager.AddToken(new ApplicationUserToken() { UserId = user.Id, LoginProvider = "MyApp", Name = TokenTypes.RefreshToken, Value = refreshToken, ExpireDate = refreshTokenExpireDate });

                    #endregion

                    if (resultForAccessToken.Success && resultForRefreshToken.Success)
                    {
                        var tokens = new TokensDto
                        {
                            AccessToken = resultForAccessToken.Data,
                            RefreshToken = resultForRefreshToken.Data
                        };
                        return new SuccessDataResult<TokensDto>(tokens, Messages.TokenProvided);
                    }

                    return new ErrorDataResult<TokensDto>(Messages.TokenNotProvided);
                }
                else
                {
                    await _userManager.AccessFailedAsync(user);

                    int failcount = await _userManager.GetAccessFailedCountAsync(user);

                    if (failcount == 3)
                    {
                        await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(1)));

                        return new ErrorDataResult<TokensDto>("Şifrənizi ard arda 3 dəfə yalnış girdiyiniz üçün hesabiniz 1 dəqiqəlik bloklandı");
                    }
                    else
                    {
                        if (signInResult.IsLockedOut)
                            return new ErrorDataResult<TokensDto>("Şifrənizi ard arda 3 dəfə yalnış girdiyiniz üçün hesabiniz 2 dəqiqəlik bloklanıb.");
                        else
                            return new ErrorDataResult<TokensDto>("E-posta veya şifre yanlışdır.");
                    }
                }
            }

            return new ErrorDataResult<TokensDto>(Messages.UserNotFound);
        }

        [ValidationAspect(typeof(UserForRegisterDtoValidator))]
        public async Task<IResult> Register(UserForRegisterDto userForRegisterDto)
        {
            var createForUser = _mapper.Map<ApplicationUser>(userForRegisterDto);

            if (await _userManager.FindByNameAsync(userForRegisterDto.UserName) != null)
                return new ErrorResult(Messages.UserAlreadyExist);

            var result = await _userManager.CreateAsync(createForUser, userForRegisterDto.Password);

            if (!result.Succeeded)
                return new ErrorResult(result.Errors.Select(s => s.Description).FirstOrDefault());

            if (!await _roleManager.RoleExistsAsync(RoleTypes.User))
                await _roleManager.CreateAsync(new ApplicationRole() { Name = RoleTypes.User });

            if (await _roleManager.RoleExistsAsync(RoleTypes.User))
                await _userManager.AddToRoleAsync(createForUser, RoleTypes.User);

            return new SuccessResult(Messages.UserCreated);
        }

        [ValidationAspect(typeof(UserForChangePasswordDtoValidator))]
        public async Task<IResult> ChangePassword(UserForChangePasswordDto userForChangePasswordDto)
        {
            var user = await _userManager.FindByIdAsync(userForChangePasswordDto.UserId);

            if (user == null)
                return new ErrorResult(Messages.UserNotFound);

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (token != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, userForChangePasswordDto.Password);

                if (result.Succeeded)
                    return new SuccessResult(Messages.PasswordReseted);
                else
                    return new ErrorResult(Messages.PasswordCanNotReseted);
            }

            return new ErrorResult(Messages.TokenNotProvided);
        }

        [ValidationAspect(typeof(TokensDtoValidator))]
        public async Task<IDataResult<TokensDto>> Refresh(TokensDto model)
        {
            var principal = _tokenHelper.GetPrincipalFromExpiredToken(model.AccessToken);
            var userName = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return new ErrorDataResult<TokensDto>(Messages.UserNotFound);


            var existTokens = await _aspNetUserTokenManager.GetTokensByUserId(user.Id);

            if (existTokens.Success)
            {
                var existAccessToken = existTokens.Data.FirstOrDefault(f => f.Name == TokenTypes.AccessToken);
                var existRefreshToken = existTokens.Data.FirstOrDefault(f => f.Name == TokenTypes.RefreshToken);


                if (user == null || existRefreshToken.Value != model.RefreshToken || existRefreshToken.ExpireDate <= DateTime.Now)
                    return new ErrorDataResult<TokensDto>(Messages.RefreshTokenExpired);


                var newAccessToken = _tokenHelper.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenHelper.GenerateRefreshToken();

                existAccessToken.Value = _tokenHelper.GenerateAccessToken(principal.Claims);
                existAccessToken.ExpireDate = new JwtSecurityToken(newAccessToken).ValidTo.ConvertUtcToLocalTime();

                existRefreshToken.Value = newRefreshToken;
                existRefreshToken.ExpireDate = DateTime.Now.AddMinutes(5);

                var uptatedAccessToken = await _aspNetUserTokenManager.UpdateToken(existAccessToken);
                var updatedRefreshToken = await _aspNetUserTokenManager.UpdateToken(existRefreshToken);

                if (uptatedAccessToken.Success && updatedRefreshToken.Success)
                {
                    var newTokens = new TokensDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
                    return new SuccessDataResult<TokensDto>(newTokens);
                }
            }

            return new ErrorDataResult<TokensDto>(Messages.TokensNotFound);
        }

        public async Task<IDataResult<bool>> Validate(string token)
        {
            if (String.IsNullOrEmpty(token))
                return new ErrorDataResult<bool>(Messages.TokenCanNotEmpty);
            
            var result = await Task.Run(() => _tokenHelper.ValidateToken(token));

            return new SuccessDataResult<bool>(result, result ? Messages.TokenValid : Messages.TokenNotValid);
        }

        public async Task<IResult> Revoke(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return new ErrorResult(Messages.UserNotFound);

            //Exist tokens find
            var existTokens = await _aspNetUserTokenManager.GetTokensByUserId(user.Id);

            if (!existTokens.Success && existTokens.Data.Count == 0)
                return new ErrorResult(existTokens.Message);

            foreach (var existToken in existTokens.Data)
            {
                existToken.Value = null;
                existToken.ExpireDate = new DateTime(1900, 1, 1);

                await _aspNetUserTokenManager.UpdateToken(existToken);
            }

            return new SuccessResult(Messages.TokensRevoked);
        }
    }
}
