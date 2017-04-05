using GoTech.Framework.Core.Configration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;

namespace GoTech.Framework.Core.Authentication
{
    public class JwtProvider
    {
        AppSetting appSetting = new AppSetting();
        public JwtProvider()
        {

        }

        public ClaimsPrincipal GetTokenClaim(string access_token)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            AuthOptions option = new AuthOptions(true);
            SecurityToken validatedToken;
            var claimsPrincipal = tokenHandler.ValidateToken(access_token, option.GetTokenValidationOptions(), out validatedToken);
            return claimsPrincipal;
        }
        public AuthResponce CreateAccessToken(string username, string role)
        {
            var _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSetting.GoTech.APISecurityKey));

            var identity = GetIdentity(username, role);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                    issuer: appSetting.GoTech.SecurityTokenIssuer,
                    audience: appSetting.GoTech.SecurityTokenAudience,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.AddDays(appSetting.GoTech.SecurityTokenExpirationDay),
                    signingCredentials: new SigningCredentials(_key, SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new AuthResponce
            {
                access_token = encodedJwt,
                Username = identity.Name
            };
            return response;
        }
        public AuthResponce CreateAccessToken(string username)
        {
            var _key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(appSetting.GoTech.APISecurityKey));

            JwtSecurityTokenHandler _tokenHandler = new JwtSecurityTokenHandler();
            var encodedJwt= _tokenHandler.CreateEncodedJwt(new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new GenericIdentity(username)),
                Expires = DateTime.UtcNow.AddDays(appSetting.GoTech.SecurityTokenExpirationDay),
                SigningCredentials = new SigningCredentials(_key, SecurityAlgorithms.HmacSha256Signature),
                Audience = appSetting.GoTech.SecurityTokenAudience,
                Issuer = appSetting.GoTech.SecurityTokenIssuer,
                
            });

            var response = new AuthResponce
            {
                access_token = encodedJwt,
                Username = username
            };
            return response;
        }


        private ClaimsIdentity GetIdentity(string username, string role)
        {
            var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, username),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }

    }
}
