
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
    public class CustomJwtFormat : ISecureDataFormat<AuthenticationTicket>
    {




        /// <summary>
        /// 
        /// </summary>
        public CustomJwtFormat()
        {

        }

       

        /// <summary>
        /// Usado por el Authorization serve
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public string Protect(AuthenticationTicket data)
        {
            if (data == null)
                throw new ArgumentNullException("data");
           
            string securityProviderName = string.Empty;
            data.Properties.Dictionary.TryGetValue("securityProviderName", out securityProviderName);
            Fwk.Security.Identity.jwtSecurityProvider sec_provider = Fwk.Security.Identity.helper.get_secConfig().GetByName(securityProviderName);
            var audienceToken = sec_provider.audienceId;
            var issuerToken = sec_provider.issuer;
            string symmetricKeyAsBase64 = sec_provider.audienceSecret;
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);
            var securityKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            //notBefore
            //var issuedAt = data.Properties.IssuedUtc;
            //var expireTime = data.Properties.ExpiresUtc;
            var expireTime = sec_provider.expires;//ConfigurationManager.AppSettings["JWT_EXPIRE_MINUTES"];
            //var token = new JwtSecurityToken(issuerToken, audienceToken, data.Identity.Claims, issuedAt.Value.UtcDateTime, expireTime.Value.UtcDateTime, signingCredentials);

            //var handler = new JwtSecurityTokenHandler();

            //var jwt = handler.WriteToken(token);

            #region otra manera de generar  el token
            // create a claimsIdentity
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, data.Properties.Dictionary["profesionalName"].ToString()) });

            claimsIdentity.AddClaim(new Claim(ClaimTypes.WindowsAccountName, data.Properties.Dictionary["userName"].ToString()));
            claimsIdentity.AddClaim(new Claim("userName", data.Properties.Dictionary["userName"].ToString()));
            claimsIdentity.AddClaim(new Claim("userId", data.Properties.Dictionary["userId"].ToString()));
            if(data.Properties.Dictionary.ContainsKey("email")!=false)
                claimsIdentity.AddClaim(new Claim("email", data.Properties.Dictionary["email"].ToString()));
            //data.Identity.Claims("role");
            //Fwk.HelperFunctions.FormatFunctions.GetStringBuilderWhitSeparator(roles, ',').ToString()
            claimsIdentity.AddClaim(new Claim("roles", data.Properties.Dictionary["roles"].ToString()));

            var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.CreateJwtSecurityToken(
                   audience: audienceToken,
                   issuer: issuerToken,
                   subject: claimsIdentity,
                   notBefore: DateTime.UtcNow, //antes no
                   issuedAt: DateTime.UtcNow,
                   expires: DateTime.UtcNow.AddMinutes(Convert.ToInt32(expireTime)),
                   signingCredentials: signingCredentials);

            var jwtTokenString = tokenHandler.WriteToken(jwtSecurityToken);
            #endregion

            return jwtTokenString;
        }

        //public AuthenticationTicket Unprotect(string protectedText)
        //{
        //    throw new NotImplementedException();
        //    //return new AuthenticationTicket(identity, authenticationProperties);
        //}

        /// <summary>
        ///  method which is responsible for validation of the JWT and returning and authentication ticket:
        /// </summary>
        /// <param name = "protectedText" ></ param >
        /// < returns ></ returns >
        public AuthenticationTicket Unprotect(string protectedText)
        {

            Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
            if (string.IsNullOrWhiteSpace(protectedText))
            {
                throw new ArgumentNullException("protectedText");
            }
            //Fwk.Security.Identity.jwtSecurityProvider sec_provider = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(protectedText);
            var securityProviderNameClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "securityProviderName");

            if (securityProviderNameClaim == null)
            {
                throw new ArgumentNullException("securityProviderName claims in jwt");
            }

            var sec_provider = helper.get_secConfig().GetByName(securityProviderNameClaim.Value);

            if (sec_provider == null)
            {
                throw new ArgumentNullException("No se encuentra configurado el proveedor (securityProviderName) en securityConfig.json");
            }
            string audienceId = sec_provider.audienceId;
            string symmetricKeyAsBase64 = sec_provider.audienceSecret;
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var securityKey = new SymmetricSecurityKey(keyByteArray);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);



            //TODO : CustomJwtFormat Esta lista de issuers debe ser flexible
            ///Establezco los issuers validos
            var issuers = new List<string>()
                {
                     "http://localhost:44345/"
                };

            var validationParams = new TokenValidationParameters()
            {

                ValidAudience = sec_provider.audienceId,
                ValidIssuers = issuers,
                ValidateLifetime = true,
                ValidateAudience = true,
                ValidateIssuer = true,
                RequireSignedTokens = true,
                RequireExpirationTime = true,
                ValidateIssuerSigningKey = true,
                ClockSkew = TimeSpan.Zero,
                //IssuerSigningKeys = DefaultX509Key_Public_2048
                IssuerSigningKey = signingCredentials.Key
            };
            try
            {
                var principal = tokenHandler.ValidateToken(protectedText, validationParams, out validatedToken);


                var identity = principal.Identities.First();

                // Fill out the authenticationProperties issued and expires times if the equivalent claims are in the JWT
                var authenticationProperties = new AuthenticationProperties();

                //issued 
                if (validatedToken.ValidFrom != DateTime.MinValue)
                {
                    authenticationProperties.IssuedUtc = validatedToken.ValidFrom.ToUniversalTime();
                }
                //expires 
                if (validatedToken.ValidTo != DateTime.MinValue)
                {
                    authenticationProperties.ExpiresUtc = validatedToken.ValidTo.ToUniversalTime();
                }

                return new AuthenticationTicket(identity, authenticationProperties);
            }
            catch (Exception ex)
            {
                //var statusCode = HttpStatusCode.Unauthorized;
                //return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
                var ec = new Fwk.Exceptions.FunctionalException((int)HttpStatusCode.Unauthorized, " No autorizado ", ex);
                throw ec;
            }




        }
    }

}
