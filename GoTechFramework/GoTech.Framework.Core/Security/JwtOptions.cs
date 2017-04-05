using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core.Security
{
    public class JwtOptions
    {
        public string Secret { get; set; }
        public int Validity { get; set; }

        public SymmetricSecurityKey SecurityKey => new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Secret));
    }
}
