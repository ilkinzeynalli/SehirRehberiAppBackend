using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.WebApi.Dtos
{
    public class TokensForRefreshDTO
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
