using GoTech.Framework.Core.Configration;
using Microsoft.AspNetCore.Builder;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core.Authentication
{
    public class AuthOptions
    {
        private bool AutomaticAuthenticate;

        public AuthOptions(bool AutomaticAuthenticate)
        {
            this.AutomaticAuthenticate = AutomaticAuthenticate;
        }
        public JwtBearerOptions GetJwtBearerOptions()
        {
            var tokenValidationParameters = GetTokenValidationOptions();
            var option = new JwtBearerOptions
            {
                AutomaticAuthenticate = true,
                AutomaticChallenge = true,
                TokenValidationParameters = tokenValidationParameters
            };
            return option;
        }
        public TokenValidationParameters GetTokenValidationOptions()
        {
            AppSetting appSetting = new AppSetting();
            var _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSetting.GoTech.APISecurityKey));
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = _key,

                ValidateIssuer = true,
                ValidIssuer = appSetting.GoTech.SecurityTokenIssuer,

                ValidateAudience = true,
                ValidAudience = appSetting.GoTech.SecurityTokenAudience,

                ValidateLifetime = true
            };
            return tokenValidationParameters;
        }
        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            AppSetting appSetting = new AppSetting();
            string Key = appSetting.GoTech.APISecurityKey;
            return new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Key));
        }
    }
}
