using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using CentralizedSecurity.webApi.Models;
using Fwk.CentralizedSecurity.Contracts;
using Fwk.Security.Identity;
using Fwk.Security.Common;

namespace CentralizedSecurity.webApi.Controllers
{
    /// <summary>
    /// JWT Token generator class using "secret-key"
    /// more info: https://self-issued.info/docs/draft-ietf-oauth-json-web-token.html
    /// </summary>
    internal static class TokenGenerator
    {
        public static string GenerateTokenJwt(EmpleadoReseteoBE empleado)
        {
            // appsetting for Token JWT
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            // audiencia quien genera el tocken
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            //identifica quien consume y uusa el tocken : El cliente
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            //hora de caducidad a partir de la cual el JWT NO DEBE ser aceptado para su procesamiento.
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, empleado.WindowsUser) });

            claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, empleado.WindowsUser));
            claimsIdentity.AddClaim(new Claim("Emp_id", empleado.Emp_id.ToString()));
            claimsIdentity.AddClaim(new Claim("Legajo", empleado.Legajo.ToString()));
            claimsIdentity.AddClaim(new Claim("dom_id", empleado.DomainId.ToString()));
            claimsIdentity.AddClaim(new Claim("cuenta", empleado.Cuenta));
            claimsIdentity.AddClaim(new Claim("cargo", empleado.Cargo));
            claimsIdentity.AddClaim(new Claim("CAIS", empleado.CAIS.ToString()));
            claimsIdentity.AddClaim(new Claim("isRessetUser", empleado.isRessetUser.ToString()));

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

        /// <summary>
        /// Retorna informacion basada en el dominio
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="aDUser"></param>
        /// <param name="groups"></param>
        /// <returns></returns>
        public static string GenerateTokenJwt_LDAP(string userName, ActiveDirectoryUser aDUser, List<ActiveDirectoryGroup> groups)
        {
            // appsetting for Token JWT
            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            // audiencia quien genera el tocken
            var audienceToken = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            //identifica quien consume y uusa el tocken : El cliente
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            //hora de caducidad a partir de la cual el JWT NO DEBE ser aceptado para su procesamiento.
            var expireTime = ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, userName) });

            claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, userName));

            if (aDUser != null )
            {
                claimsIdentity.AddClaim(new Claim("FirstName", aDUser.FirstName.ToString()));
                claimsIdentity.AddClaim(new Claim("UserAccountControl", aDUser.UserAccountControl));
                claimsIdentity.AddClaim(new Claim("Department", aDUser.Department));
                claimsIdentity.AddClaim(new Claim("LastName", aDUser.LastName));
                claimsIdentity.AddClaim(new Claim("LoginNameWithDomain", aDUser.LoginNameWithDomain));
                claimsIdentity.AddClaim(new Claim("EmailAddress", aDUser.EmailAddress));
            }
            if (groups != null && groups.Count() != 0)
            {
                var roles = Fwk.HelperFunctions.FormatFunctions.GetStringBuilderWhitSeparator(groups, ',').ToString();
                claimsIdentity.AddClaim(new Claim("groups", roles.ToString()));
            }
           


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



        public static string GenerateTokenJwt_ApiBot(User user,string securityProviderName)
        {
            var sec_provider = helper.get_secConfig().GetByName(securityProviderName);
            // appsetting for Token JWT
            var secretKey = sec_provider.audienceSecret;
            // audiencia quien genera el tocken
            var audienceToken = sec_provider.audienceId;
            //identifica quien consume y uusa el tocken : El cliente
            var issuerToken = sec_provider.issuer;
            //hora de caducidad a partir de la cual el JWT NO DEBE ser aceptado para su procesamiento.
            var expireTime = sec_provider.expires;

            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.UserName) });

            claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, user.UserName));
            claimsIdentity.AddClaim(new Claim("userId", user.ProviderId.ToString()));
            

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
