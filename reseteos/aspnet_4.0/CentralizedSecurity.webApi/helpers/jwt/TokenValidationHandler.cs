using System;
using System.Collections.Generic;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;

namespace CentralizedSecurity.webApi.Controllers
{

    /// <summary>
    /// Utilizado por Dispatcher para recibir peticiones (header bearer) 
    /// </summary>
    internal class TokenValidationHandler : DelegatingHandler
    {
        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
        {
            token = null;
            IEnumerable<string> authzHeaders;
            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
            {
                return false;
            }
            var bearerToken = authzHeaders.ElementAt(0);
            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
            return true;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            HttpStatusCode statusCode;
            string errorMessage=string.Empty;
            string token;
            HttpResponseMessage httpResponse=null;
            // determine whether a jwt exists or not
            if (!TryRetrieveToken(request, out token))
            {
                 httpResponse = new HttpResponseMessage(HttpStatusCode.Unauthorized)
                {
                    Content = new StringContent("You have no token"),
                    ReasonPhrase = "Error"
                };
            
                return Task<HttpResponseMessage>.Factory.StartNew(() => httpResponse);
            }

            try
            {
                Unprotect(token);

               

                return base.SendAsync(request, cancellationToken);
            }
            catch (SecurityTokenValidationException seErr)
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = seErr.Message;
            }
            catch (FormatException fx)
            {
                statusCode = HttpStatusCode.Unauthorized;
                errorMessage = "Invalid token";
                errorMessage = Environment.NewLine + fx.Message;
            }
            catch (Exception ex)
            {
                statusCode = HttpStatusCode.InternalServerError;
                errorMessage = Environment.NewLine + ex.Message;
                httpResponse = apiHelper.fromEx(ex);
            }
            if(httpResponse==null)
                httpResponse = apiHelper.fromErrorString(errorMessage, statusCode);
            
            return Task<HttpResponseMessage>.Factory.StartNew(() => httpResponse);
        }


        static AuthenticationTicket Unprotect(string protectedText)
        {
            
            Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
            if (string.IsNullOrWhiteSpace(protectedText))
            {
                throw new ArgumentNullException("protectedText");
            }
            //Fwk.Security.Identity.jwtSecurityProvider sec_provider = null;

            var tokenHandler = new JwtSecurityTokenHandler();
            var jwtSecurityToken = tokenHandler.ReadJwtToken(protectedText);

            var secretKey = ConfigurationManager.AppSettings["JWT_SECRET_KEY"];
            var audienceId = ConfigurationManager.AppSettings["JWT_AUDIENCE_TOKEN"];
            var issuerToken = ConfigurationManager.AppSettings["JWT_ISSUER_TOKEN"];
            var securityKey = new SymmetricSecurityKey(System.Text.Encoding.Default.GetBytes(secretKey));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            //string audienceId = sec_provider.audienceId;
            //string symmetricKeyAsBase64 = sec_provider.audienceSecret;
            //var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            //var securityKey = new SymmetricSecurityKey(keyByteArray);




            //TODO : CustomJwtFormat Esta lista de issuers debe ser flexible
            ///Establezco los issuers validos
            var issuers = new List<string>()
                {
                    "pelsoft",
                    "issuerA",
                    "issuerB",
                     "http://localhost:50009"
                };

            var validationParams = new TokenValidationParameters()
            {

                ValidAudience = audienceId,
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
                throw new UnauthorizedAccessException(ex.Message);
                //throw ex;
            }
        }

        public bool LifetimeValidator(DateTime? notBefore, DateTime? expires, SecurityToken securityToken, TokenValidationParameters validationParameters)
        {
            if (expires != null)
            {
                if (DateTime.UtcNow < expires) return true;
            }
            return false;
        }
    }
}