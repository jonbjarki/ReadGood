using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ReadTogether.Domain.Configuration
{
    public class JwtConfiguration
    {
        public required string Key { get; set; } = "";
        public required string Issuer { get; set; } = "";
        public required string Audience { get; set; } = "";
        public required int ExpiresInHours { get; set; }

    }
}