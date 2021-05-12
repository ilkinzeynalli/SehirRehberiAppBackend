using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Attributes;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.DTOs;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("tokens")]
    [Authorize]
    [ApiController]
    public class TokensController : ControllerBase
    {
        private readonly IAuthService _authManager;
        public TokensController(IAuthService authManager)
        {
            _authManager = authManager;
        }

        [AllowAnonymous]
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] TokensDto model)
        {
            var result =  await _authManager.Refresh(model);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [AllowAnonymous]
        [HttpGet("validate")]
        public async Task<IActionResult> Validate([FromQuery] string token)
        {
            var result = await _authManager.Validate(token);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }

        [HttpGet("revoke")]
        public async Task<IActionResult> Revoke()
        {
            var userName = User.Identity.Name;

            if (String.IsNullOrEmpty(userName))
                return NotFound(Messages.UserNotFound);

            var result = await _authManager.Revoke(userName);

            if (result.Success)
                return Ok(result);

            return BadRequest(result);
        }
    }
}
