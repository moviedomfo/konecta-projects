using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CentralizedSecurity.webApi.Models;
using Fwk.Security.Identity;
using Fwk.Security.Common;

namespace CentralizedSecurity.webApi.Controllers
{
    //http://localhost:50010/Swagger/ui/index#/api

        /// <summary>
        /// Apis de meucci
        /// </summary>
    [CustomExceptionFilter]
    [RoutePrefix("api/epiron")]
    public class EpironController : ApiController
    {
        /// <summary>
        /// Api de autenticacion que genera el jwt.- Realiza validaciones contra las memberships configuradas
        /// 
        /// Es utilizada por el momento por Epiron
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Token tjw</returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public HttpResponseMessage Authenticate(LoginRequest login)
        {
            if (login == null)
                return apiHelper.fromEx(new HttpResponseException(HttpStatusCode.BadRequest));

            if (string.IsNullOrEmpty(login.username))
                return apiHelper.fromEx(new HttpResponseException(HttpStatusCode.BadRequest));


            try
            {
                var sec_provider = helper.get_secConfig().GetByName(login.securityProviderName);
                if (sec_provider == null)
                {
                    throw apiHelper.fromErrorString2(String.Format("El porveedor de seguridad {0} provisto no es correcto  ", login.securityProviderName), HttpStatusCode.InternalServerError);
                }

                User user =null;
                if (Fwk.Security.FwkMembership.ValidateUser(login.username, login.password, sec_provider.securityModelContext))
                {
                    user = Fwk.Security.FwkMembership.GetUser(login.username, sec_provider.securityModelContext);
                }

                if (user==null)
                {
                    throw apiHelper.fromErrorString2("El usuario no resitrado  " + login.domain, HttpStatusCode.Unauthorized);
                }

                var token = TokenGenerator.GenerateTokenJwt_ApiBot(user, login.securityProviderName);

                return apiHelper.fromObject<string>(token);

            }
            catch (Fwk.Exceptions.TechnicalException ex)
            {
                if (ex.ErrorId == "4013") 
                    throw apiHelper.fromEx2(ex,  HttpStatusCode.Unauthorized);

                throw apiHelper.fromEx2(ex);
            }
            //catch (Exception ex)
            //{
            //    throw apiHelper.fromEx2(ex);
                
            //}



        }


    }
}