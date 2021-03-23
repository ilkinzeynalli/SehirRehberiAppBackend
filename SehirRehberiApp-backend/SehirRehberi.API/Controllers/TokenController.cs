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
using SehirRehberi.DataAccess.Abstract;
using SehirRehberi.Entities.Concrete;
using SehirRehberi.WebApi.Attributes;
using SehirRehberi.WebApi.Dtos;
using SehirRehberi.WebApi.Extensions;
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
        public async Task<IActionResult> Refresh([FromBody] TokensForRefreshDTO model)
        {
            var principal = _tokenService.GetPrincipalFromExpiredToken(model.AccessToken);
            var userName = principal.Identity.Name; //this is mapped to the Name claim by default

            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
                return NotFound("User not found");


            var existTokens = await _aspNetUserTokenService.GetTokensByUserId(user.Id);

            var existAccessToken = existTokens.FirstOrDefault(f => f.Name == TokenTypes.AccessToken);
            var existRefreshToken = existTokens.FirstOrDefault(f => f.Name == TokenTypes.RefreshToken);

            if (user == null || existRefreshToken.Value != model.RefreshToken || existRefreshToken.ExpireDate <= DateTime.Now)
                return Unauthorized("Refresh token expired");


            var newAccessToken = _tokenService.GenerateAccessToken(principal.Claims);
            var newRefreshToken = _tokenService.GenerateRefreshToken();

            existAccessToken.Value = _tokenService.GenerateAccessToken(principal.Claims);
            existAccessToken.ExpireDate = new JwtSecurityToken(newAccessToken).ValidTo.ConvertUtcToLocalTime();

            existRefreshToken.Value = newRefreshToken;
            existRefreshToken.ExpireDate = DateTime.Now.AddMinutes(5);

            await _aspNetUserTokenService.UpdateToken(existAccessToken);
            await _aspNetUserTokenService.UpdateToken(existRefreshToken);

            return Ok(new{ newAccessToken, newRefreshToken });

        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate([FromQuery] string token)
        {
            var result = await Task.Run(() => _tokenService.ValidateToken(token));

            return Ok(result);
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

            if (existTokens.Count == 0)
                return NotFound("Tokens not found");

            foreach (var existToken in existTokens)
            {
                existToken.Value = null;
                existToken.ExpireDate = new DateTime(1900, 1, 1);

                await _aspNetUserTokenService.UpdateToken(existToken);
            }

            return Ok("Tokens revoked");
        }
    }
}
