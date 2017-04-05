using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core.Security
{
    public class JwtSettings
    {
        public SymmetricSecurityKey SecurityKey { get; }
        public int TokenExpiration { get; }
        public string Audience { get; } = "GoTechAudience";
        public string Issuer { get; } = "GoTechIssuer";

        public JwtSettings(SymmetricSecurityKey securityKey, int tokenExpiration)
        {
            SecurityKey = securityKey;
            TokenExpiration = tokenExpiration;
        }
    }
}
