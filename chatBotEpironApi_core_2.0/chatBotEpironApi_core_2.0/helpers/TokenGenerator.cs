using chatBotEpironApi.webApi.Models;
using Fwk.chatBotEpironApi.Contracts;
using Fwk.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace chatBotEpironApi.webApi.helpers
{
    public class TokenGenerator
    {

        public static  string GenerateTokenEpiron(UserAPiBE emmpleadoBE)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(apiAppSettings.serverSettings.apiConfig.api_secretKey);

            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, emmpleadoBE.WindowsUser) });
            if (emmpleadoBE != null)
            {
                claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, emmpleadoBE.WindowsUser));
                claimsIdentity.AddClaim(new Claim("Emp_id", emmpleadoBE.Emp_id.ToString()));
                claimsIdentity.AddClaim(new Claim("Legajo", emmpleadoBE.Legajo.ToString()));
                claimsIdentity.AddClaim(new Claim("dom_id", emmpleadoBE.DomainId.ToString()));
                claimsIdentity.AddClaim(new Claim("cuenta", emmpleadoBE.Cuenta));
                claimsIdentity.AddClaim(new Claim("cargo", emmpleadoBE.Cargo));
                claimsIdentity.AddClaim(new Claim("CAIS", emmpleadoBE.CAIS.ToString()));
            }


            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Audience = apiAppSettings.serverSettings.apiConfig.api_audienceToken,
                Subject = claimsIdentity,
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(apiAppSettings.serverSettings.apiConfig.api_expireTime)),

                Issuer = apiAppSettings.serverSettings.apiConfig.api_issuerToken,
                NotBefore = DateTime.UtcNow,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };


            var secToken = tokenHandler.CreateToken(tokenDescriptor);
            var jwtTokenString = tokenHandler.WriteToken(secToken);
            return jwtTokenString;
        }

      

        /// <summary>
        /// Retorna informacion basada en el dominio
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public static string GenerateTokenJwt_test(string userName)
        {
            // appsetting for Token JWT

                               
               var secretKey = apiAppSettings.serverSettings.apiConfig.api_secretKey;
            // audiencia quien genera el tocken
            var audienceToken = apiAppSettings.serverSettings.apiConfig.api_audienceToken;
            //identifica quien consume y uusa el tocken : El cliente
            var issuerToken = apiAppSettings.serverSettings.apiConfig.api_issuerToken;
            //hora de caducidad a partir de la cual el JWT NO DEBE ser aceptado para su procesamiento.
            var expireTime = apiAppSettings.serverSettings.apiConfig.api_expireTime;

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) });

            claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, userName));

            //if (aDUser != null)
            //{
            //    claimsIdentity.AddClaim(new Claim("FirstName", userName));
            //    claimsIdentity.AddClaim(new Claim("UserAccountControl", aDUser.UserAccountControl));
            //    claimsIdentity.AddClaim(new Claim("Department", aDUser.Department));
            //    claimsIdentity.AddClaim(new Claim("LastName", aDUser.LastName));
            //    claimsIdentity.AddClaim(new Claim("LoginNameWithDomain", aDUser.LoginNameWithDomain));
            //    claimsIdentity.AddClaim(new Claim("EmailAddress", aDUser.EmailAddress));
            //}
            //if (groups != null && groups.Count != 0)
            //{
            //    var roles = Fwk.HelperFunctions.FormatFunctions.GetStringBuilderWhitSeparator(groups, ',').ToString();
            //    claimsIdentity.AddClaim(new Claim("groups", roles.ToString()));
            //}



            // create token to the user
            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                audience: audienceToken,
                issuer: issuerToken,
                subject: claimsIdentity,
                notBefore: DateTime.UtcNow,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            return jwtTokenString;
        }
    }
}
