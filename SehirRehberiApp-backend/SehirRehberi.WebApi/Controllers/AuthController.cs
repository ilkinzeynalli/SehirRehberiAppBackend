using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using SehirRehberi.Business.Abstract;
using SehirRehberi.Business.Constants;
using SehirRehberi.Core.Attributes;
using SehirRehberi.Core.Utilities.Results;
using SehirRehberi.Entities.DTOs;

namespace SehirRehberi.WebApi.Controllers
{
    [ApiRoutePrefix("auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController( IAuthService authServie)
        {
            _authService = authServie;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Login(userForLoginDto);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest(new ErrorResult(Messages.ModelNotValid));
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDTO)
        {
            if (ModelState.IsValid)
            {
                var result = await _authService.Register(userForRegisterDTO);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest(Messages.ModelNotValid);
        }

        [HttpPut("changePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserForChangePasswordDto userForChangePasswordDTO)
        {

            if (ModelState.IsValid)
            {
                var result = await _authService.ChangePassword(userForChangePasswordDTO);

                if (result.Success)
                {
                    return Ok(result);
                }

                return BadRequest(result);
            }

            return BadRequest(Messages.ModelNotValid);
        }
    }
}
