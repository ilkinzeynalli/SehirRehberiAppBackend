using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using SehirRehberi.Core.Extensions;
using SehirRehberi.WebApi.Modules;
using SehirRehberi.WebApi.Services.Abstract;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.Services.Concrete
{
    public class TokenManager : ITokenService
    {
        private readonly IConfiguration _configration;
        public TokenManager(IConfiguration configration)
        {
            _configration = configration;
        }

        public string GenerateAccessToken(IEnumerable<Claim> claims)
        {
            var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configration["JWT:SecretKey"]));
            var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

            var tokeOptions = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(2),
                signingCredentials: signinCredentials
            );

            var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
            return tokenString;
        }

        public string GenerateRefreshToken()
        {
            var randomNumber = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(randomNumber);
                return Convert.ToBase64String(randomNumber);
            }
        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var tokenValidationParameters = JwtModule.GetTokenValidationParameters(_configration);
            var tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out securityToken);
            var jwtSecurityToken = securityToken as JwtSecurityToken;
            if (jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                throw new SecurityTokenException("Invalid token");
            return principal;
        }

        public bool ValidateToken(string token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenValidationParameters = JwtModule.GetTokenValidationParameters(_configration);
            var jwtToken = new JwtSecurityToken(token);

            SecurityToken validatedToken = null;

            try
            {
                tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenException)
            {
                return false;
            }

            return validatedToken != null && ((jwtToken != null) && (jwtToken.ValidTo.ConvertUtcToLocalTime() >= DateTime.Now));
        }

       
    }
}
