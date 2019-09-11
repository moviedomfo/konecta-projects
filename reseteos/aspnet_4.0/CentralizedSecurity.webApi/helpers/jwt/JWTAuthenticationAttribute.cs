﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Net.Http;
using System.Net;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Owin.Security;
using System.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Web.Http;

namespace CentralizedSecurity.webApi.helpers
{
    public class JWTActionAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            if (actionExecutedContext.Request.Headers.Authorization == null)
            {
                actionExecutedContext.Response = actionExecutedContext.Request.CreateResponse(HttpStatusCode.Unauthorized, "token not found");
            }

        }
    }

    public class JWTAuthenticationAttribute : AuthorizationFilterAttribute
    {
     
        public override void OnAuthorization(HttpActionContext actionContext)
        {
            

            if (SkipAuthorization(actionContext))
            {
                return;
            }
            HttpStatusCode statusCode= HttpStatusCode.OK;
            string authToken = string.Empty;
            string errorMessage = string.Empty;
            if (actionContext.Request.Headers.Authorization == null)
            {
                actionContext.Response = actionContext.Request.CreateResponse(HttpStatusCode.Unauthorized,"Token not found");
            }
            else
            {
                 authToken = actionContext.Request.Headers.Authorization.Parameter;
            }

            try
            {
                Unprotect(authToken);
                //authorized
          
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
        
            }
            if (statusCode !=  HttpStatusCode.OK)
                actionContext.Response = actionContext.Request.CreateResponse(statusCode, errorMessage);

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


        private static bool SkipAuthorization(HttpActionContext actionContext)
        {
            if (!Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ActionDescriptor.GetCustomAttributes<AllowAnonymousAttribute>()))
                return Enumerable.Any<AllowAnonymousAttribute>((IEnumerable<AllowAnonymousAttribute>)actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowAnonymousAttribute>());
            else
                return true;
        }
    }
}