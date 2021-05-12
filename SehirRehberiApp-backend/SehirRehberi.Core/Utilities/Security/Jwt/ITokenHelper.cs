using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;

namespace SehirRehberi.Core.Utilities.Security.Jwt
{
    public interface ITokenHelper
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token);
        bool ValidateToken(string token);
    }
}
