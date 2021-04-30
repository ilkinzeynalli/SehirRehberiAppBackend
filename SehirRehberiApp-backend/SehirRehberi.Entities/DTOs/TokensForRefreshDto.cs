using SehirRehberi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SehirRehberi.Entities.DTOs
{
    public class TokensForRefreshDto: IDto
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
    }
}
