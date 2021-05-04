using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Extensions;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.Entities.DTOs;
using SehirRehberi.WebApi.Attributes;
using SehirRehberi.WebApi.Models;
using SehirRehberi.WebApi.Services.Abstract;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("token")]
    [Authorize]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAspNetUserTokenService _aspNetUserTokenService;
        private readonly ITokenService _tokenService;
        public TokenController(UserManager<ApplicationUser> userManager, IAspNetUserTokenService aspNetUserTokenService, ITokenService tokenService)
        {
            _userManager = userManager;
            _aspNetUserTokenService = aspNetUserTokenService;
            _tokenService = tokenService;
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokensForRefreshDto model)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(model.AccessToken);
            var userName = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound(new ErrorResult(Messages.UserNotFound));


            var existTokens = await _aspNetUserTokenService.GetTokensByUserId(user.Id);

            if (existTokens.Success)
            {

                var existAccessToken = existTokens.Data.FirstOrDefault(f => f.Name == TokenTypes.AccessToken);
                var existRefreshToken = existTokens.Data.FirstOrDefault(f => f.Name == TokenTypes.RefreshToken);


                if (user == null || existRefreshToken.Value != model.RefreshToken || existRefreshToken.ExpireDate <= DateTime.Now)
                    return Unauthorized(new ErrorResult(Messages.RefreshTokenExpired));


                var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
                var newRefreshToken = _tokenService.GenerateRefreshToken();

                existAccessToken.Value = _tokenService.GenerateAccessToken(principal.Claims);
                existAccessToken.ExpireDate = new JwtSecurityToken(newAccessToken).ValidTo.ConvertUtcToLocalTime();

                existRefreshToken.Value = newRefreshToken;
                existRefreshToken.ExpireDate = DateTime.Now.AddMinutes(5);

                var uptatedAccessToken = await _aspNetUserTokenService.UpdateToken(existAccessToken);
                var updatedRefreshToken = await _aspNetUserTokenService.UpdateToken(existRefreshToken);

                if (uptatedAccessToken.Success && updatedRefreshToken.Success)
                {
                    var newTokens = new TokensForRefreshDto { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
                    return Ok(new SuccessDataResult<TokensForRefreshDto>(newTokens));
                }
            }

            return BadRequest(existTokens);

        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate([FromQuery] string token)
        {
            var result = await Task.Run(() => _tokenService.ValidateToken(token));

            return Ok(new SuccessDataResult<bool>(result,result ? Messages.TokenValid:Messages.TokenNotValid));
        }

        [HttpPost]
        [Route("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var userName = User.Identity.Name;
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null) return BadRequest();

            //Exist tokens find
            var existTokens = await _aspNetUserTokenService.GetTokensByUserId(user.Id);

            if (!existTokens.Success && existTokens.Data.Count == 0)
                return NotFound(existTokens);

            foreach (var existToken in existTokens.Data)
            {
                existToken.Value = null;
                existToken.ExpireDate = new DateTime(1900, 1, 1);

                await _aspNetUserTokenService.UpdateToken(existToken);
            }

            return Ok(new SuccessResult(Messages.TokensRevoked));
        }
    }
}
