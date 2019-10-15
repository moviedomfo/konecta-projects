//using System;

//using System.Collections.Generic;
//using System.Configuration;
//using System.Diagnostics;
//using System.IdentityModel.Tokens.Jwt;
//using System.Linq;
//using System.Net;
//using System.Net.Http;
//using System.Security.Claims;
//using System.Threading;
//using System.Threading.Tasks;
//using System.Web;
//using Fwk.Security.Identity;
//using Microsoft.IdentityModel.Tokens;
//using Microsoft.Owin.Security;
//using Microsoft.Owin.Security.DataHandler.Encoder;

//namespace CentralizedSecurity.webApi.helpers.jwt
//{

//    /// <summary>
//    /// Interset api calls
//    /// </summary>
//    public class JwtAuthHandler : DelegatingHandler
//    {

//        /// <summary>
//        /// Este metodo intersepta antes de procesar la llamada a la api 
//        /// permite leer elheader de forma global
//        /// </summary>
//        /// <param name="request"></param>
//        /// <param name="cancellationToken"></param>
//        /// <returns></returns>
//        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        {
//            HttpStatusCode statusCode;
//            generateWhiteList();
//            HttpResponseMessage wHttpResponseMessage;
//            try
//            {
//                //request.
//                //if (whiteList.Any(s => s.Equals(req.serviceName)) == false)
//                //    return String.Format("Error the service {0} was not allowed", req.serviceName);
//                IEnumerable<string> authHeaderValues;
//                request.Headers.TryGetValues("Authorization", out authHeaderValues);


//                if (authHeaderValues == null)
//                    return base.SendAsync(request, cancellationToken); // cross fingers

//                var bearerToken = authHeaderValues.ElementAt(0);
//                var token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;

//                Unprotect(token);
//                //var secret = "";//WebConfigurationManager.AppSettings.Get("jwtKey");
//                //Thread.CurrentPrincipal = ValidateToken(
//                //    token,
//                //    secret,
//                //    true
//                //    );

//                //if (HttpContext.Current != null)
//                //{
//                //    HttpContext.Current.User = Thread.CurrentPrincipal;
//                //}

//                return base.SendAsync(request, cancellationToken);
//            }
//            catch (SecurityTokenValidationException sEx)
//            {
//                //statusCode = HttpStatusCode.Unauthorized;
//                wHttpResponseMessage = apiHelper.fromEx(sEx);
//            }
//            catch (Exception ex)
//            {
//                //statusCode = HttpStatusCode.InternalServerError;
//                wHttpResponseMessage = apiHelper.fromEx(ex);
//            }

//            return Task<HttpResponseMessage>.Factory.StartNew(() => wHttpResponseMessage);
//        }


//        private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
//        {
//            token = null;
//            IEnumerable<string> authzHeaders;
//            if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
//            {
//                return false;
//            }
//            var bearerToken = authzHeaders.ElementAt(0);
//            token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
//            return true;
//        }
//        List<String> whiteList;
//        void generateWhiteList()
//        {
//            if (whiteList == null)
//            {
//                whiteList = new List<string>();
//                whiteList.Add("AuthenticateUserService");
//                whiteList.Add("SearchParametroByParamsService");
//                whiteList.Add("GetHealthInstitutionByIdService");
//                whiteList.Add("RetriveHealthInstitutionService");

//            }
//        }




//        public AuthenticationTicket Unprotect(string protectedText)
//        {

//            Microsoft.IdentityModel.Tokens.SecurityToken validatedToken;
//            if (string.IsNullOrWhiteSpace(protectedText))
//            {
//                throw new ArgumentNullException("protectedText");
//            }
//            //Fwk.Security.Identity.jwtSecurityProvider sec_provider = null;

//            var tokenHandler = new JwtSecurityTokenHandler();
//            var jwtSecurityToken = tokenHandler.ReadJwtToken(protectedText);
//            var securityProviderNameClaim = jwtSecurityToken.Claims.FirstOrDefault(c => c.Type == "securityProviderName");

//            if (securityProviderNameClaim == null)
//            {
//                throw new ArgumentNullException("securityProviderName claims in jwt");
//            }

//            var sec_provider = helper.get_secConfig().GetByName(securityProviderNameClaim.Value);

//            if (sec_provider == null)
//            {
//                throw new ArgumentNullException("No se encuentra configurado el proveedor (securityProviderName) en securityConfig.json");
//            }
//            string audienceId = sec_provider.audienceId;
//            string symmetricKeyAsBase64 = sec_provider.audienceSecret;
//            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

//            var securityKey = new SymmetricSecurityKey(keyByteArray);
//            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);



//            //TODO : CustomJwtFormat Esta lista de issuers debe ser flexible
//            ///Establezco los issuers validos
//            var issuers = new List<string>()
//                {
//                    "pelsoft",
//                    "issuerA",
//                    "issuerB",
//                     "http://localhost:63251"
//                };

//            var validationParams = new TokenValidationParameters()
//            {

//                ValidAudience = sec_provider.audienceId,
//                ValidIssuers = issuers,
//                ValidateLifetime = true,
//                ValidateAudience = true,
//                ValidateIssuer = true,
//                RequireSignedTokens = true,
//                RequireExpirationTime = true,
//                ValidateIssuerSigningKey = true,
//                ClockSkew = TimeSpan.Zero,
//                //IssuerSigningKeys = DefaultX509Key_Public_2048
//                IssuerSigningKey = signingCredentials.Key
//            };
//            try
//            {
//                var principal = tokenHandler.ValidateToken(protectedText, validationParams, out validatedToken);


//                var identity = principal.Identities.First();

//                // Fill out the authenticationProperties issued and expires times if the equivalent claims are in the JWT
//                var authenticationProperties = new AuthenticationProperties();

//                //issued 
//                if (validatedToken.ValidFrom != DateTime.MinValue)
//                {
//                    authenticationProperties.IssuedUtc = validatedToken.ValidFrom.ToUniversalTime();
//                }
//                //expires 
//                if (validatedToken.ValidTo != DateTime.MinValue)
//                {
//                    authenticationProperties.ExpiresUtc = validatedToken.ValidTo.ToUniversalTime();
//                }

//                return new AuthenticationTicket(identity, authenticationProperties);
//            }
//            catch (Exception ex)
//            {
//                //var statusCode = HttpStatusCode.Unauthorized;
//                //return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode) { });
//                throw new UnauthorizedAccessException(ex.Message);
//                //throw ex;
//            }
//        }
//        private static DateTime FromUnixTime(long unixTime)
//        {
//            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
//            return epoch.AddSeconds(unixTime);
//        }
//    }

//}