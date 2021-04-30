using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Core.Extensions;
using SehirRehberi.DataAccess.Concrete.EntityFramework.Contexts;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using SehirRehberi.WebApi.Attributes;
using SehirRehberi.WebApi.Models;
using SehirRehberi.WebApi.Services.Abstract;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IMapper _mapper;

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        private readonly ITokenService _tokenService;
        private readonly IAspNetUserTokenService _aspNetUserTokenSerice;


        public AuthController(IMapper mapper,
                              UserManager<ApplicationUser> userManager, 
                              RoleManager<ApplicationRole> roleManager,
                              SignInManager<ApplicationUser> signInManager,
                              ITokenService tokenService,
                              IAspNetUserTokenService aspNetUserTokenSerice)
        {
            _mapper = mapper;

            _userManager = userManager;
            _roleManager = roleManager;
            _signInManager = signInManager;

            _tokenService = tokenService;
            _aspNetUserTokenSerice = aspNetUserTokenSerice;

        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDTO)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByNameAsync(userForLoginDTO.UserName);

                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    var signInResult = await _signInManager.PasswordSignInAsync(user, userForLoginDTO.Password, false, false);

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

                        var existTokens = await _aspNetUserTokenSerice.GetTokensByUserId(user.Id);
                        if (existTokens.Data.Count > 0)
                        {
                            foreach (var removedToken in existTokens.Data)
                                await _aspNetUserTokenSerice.RemoveToken(removedToken);
                        }

                        var accessToken = _tokenService.GenerateAccessToken(claims);
                        var accessTokeExpireDate = new JwtSecurityToken(accessToken).ValidTo.ConvertUtcToLocalTime();
                        var refreshToken = _tokenService.GenerateRefreshToken();
                        var refreshTokenExpireDate = DateTime.Now.AddMinutes(5);

                        await _aspNetUserTokenSerice.AddToken(new ApplicationUserToken() { UserId = user.Id, LoginProvider = "MyApp", Name = TokenTypes.AccessToken, Value = accessToken, ExpireDate = accessTokeExpireDate });
                        await _aspNetUserTokenSerice.AddToken(new ApplicationUserToken() { UserId = user.Id, LoginProvider = "MyApp", Name = TokenTypes.RefreshToken, Value = refreshToken, ExpireDate = refreshTokenExpireDate });

                        #endregion

                        return Ok(new
                        {
                            Token = accessToken,
                            RefreshToken = refreshToken
                        });
                    }
                    else
                    {
                        await _userManager.AccessFailedAsync(user);

                        int failcount = await _userManager.GetAccessFailedCountAsync(user);

                        if (failcount == 3)
                        {
                            await _userManager.SetLockoutEndDateAsync(user, new DateTimeOffset(DateTime.Now.AddMinutes(1)));

                            ModelState.AddModelError("Locked", "Şifrənizi ard arda 3 dəfə yalnış girdiyiniz üçün hesabiniz 1 dəqiqəlik bloklandı");
                        }
                        else
                        {
                            if (signInResult.IsLockedOut)
                                ModelState.AddModelError("Locked", "Şifrənizi ard arda 3 dəfə yalnış girdiyiniz üçün hesabiniz 2 dəqiqəlik bloklanıb.");
                            else
                                ModelState.AddModelError("NotUser2", "E-posta veya şifre yanlışdır.");
                        }

                        string errorMessages = "";

                        foreach (var value in ModelState.Values)
                            foreach (var error in value.Errors)
                                errorMessages += error.ErrorMessage;

                        return Unauthorized(new { Status = 401, Message = errorMessages });
                    }
                }

                return NotFound();
            }

            return BadRequest();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDTO)
        {
            if (ModelState.IsValid)
            {
                var createForUser = _mapper.Map<ApplicationUser>(userForRegisterDTO);

                if (await _userManager.FindByNameAsync(userForRegisterDTO.UserName) != null)
                    StatusCode(500, "User already exist");

                var result = await _userManager.CreateAsync(createForUser, userForRegisterDTO.Password);

                if (!result.Succeeded)
                    return StatusCode(500, new { Message = result.Errors.Select(s => s.Description).FirstOrDefault() });

                if (!await _roleManager.RoleExistsAsync(RoleTypes.User))
                    await _roleManager.CreateAsync(new ApplicationRole() { Name = RoleTypes.User });

                if (await _roleManager.RoleExistsAsync(RoleTypes.User))
                    await _userManager.AddToRoleAsync(createForUser, RoleTypes.User);

                return StatusCode(201);
            }

            return BadRequest();
        }

        [HttpPut]
        [Route("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserForChangePasswordDto userForChangePasswordDTO)
        {
            var user = await _userManager.FindByIdAsync(userForChangePasswordDTO.UserId);

            if (user == null)
                return NotFound("Istifadeci tapilmadi");

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            if (token != null)
            {
                var result = await _userManager.ResetPasswordAsync(user, token, userForChangePasswordDTO.Password);

                if (result.Succeeded)
                    return Ok();
                else
                    return StatusCode(500, new { Status = 500, Message = "Şifrə sıfırlanmadı.Şifrənizi kontrol edin" });
            }

            return StatusCode(500, new { Status = 500, Message = "Token atanirken problem yaşandı" });
        }
    }
}
