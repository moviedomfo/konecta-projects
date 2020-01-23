using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System;
using System.Linq;

using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http;
using Fwk.Security.Identity;
using System.Security.Claims;
using System.Configuration;

namespace fwk.security.webApi
{
    /// <summary>
    /// JWT Token generator class using "secret-key"
    /// more info: https://self-issued.info/docs/draft-ietf-oauth-json-web-token.html
    /// </summary>
    public static class TokenGenerator
    {
        public static string GenerateTokenJwt(Guid userId, string userName, string email, List<string> rolesArray, string securityProviderName)
        {
            var sec_provider = helper.get_secConfig().GetByName(securityProviderName);

            if (sec_provider == null)
            {
                throw new ArgumentNullException("No se encuentra configurado el proveedor (securityProviderName) en securityConfig.json");
            }

            string symmetricKeyAsBase64 = sec_provider.audienceSecret;
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var audienceToken = sec_provider.audienceId;
            var issuerToken = sec_provider.issuer;
            var securityKey = new SymmetricSecurityKey(keyByteArray);
            
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var expireTime = 600;// ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];


            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, "") });

            //claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, username));
            claimsIdentity.AddClaim(new Claim("userName", userName));
            claimsIdentity.AddClaim(new Claim("userId", userId.ToString()));
            if (!string.IsNullOrEmpty(email))
                claimsIdentity.AddClaim(new Claim("email", email));

            if (rolesArray != null && rolesArray.Count() != 0)
            {
                var roles = Fwk.HelperFunctions.FormatFunctions.GetStringBuilderWhitSeparator(rolesArray, ',').ToString();
                claimsIdentity.AddClaim(new Claim("roles", roles.ToString()));
            }


            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow.AddMinutes(-10), //antes no para System.IdentityModel.Tokens. es ValidFrom
                   issuedAt: DateTime.UtcNow,
                   expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}