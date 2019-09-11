using System;
using System.Net;
using System.Web.Http;
using CentralizedSecurity.webApi.Models;
using Fwk.CentralizedSecurity.helpers;
using CentralizedSecurity.webApi.DAC;
using System.Web.Http.Cors;
using System.Net.Http;

namespace CentralizedSecurity.webApi.Controllers
{
    [AllowAnonymous]
    [RoutePrefix("api/oauth")]
    public class AuthController : ApiController
    {
        //[AllowCrossSiteJson]
        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public IHttpActionResult Ping()
        {
            return Ok(true);
        }

      

        [HttpPost]
        [Route("authenticate")]
        public HttpResponseMessage Authenticate(LoginRequest login)
        {
            if (login == null)
                return apiHelper.fromEx(new HttpResponseException(HttpStatusCode.BadRequest));

            if (string.IsNullOrEmpty (login.username ))
                return apiHelper.fromEx(new HttpResponseException(HttpStatusCode.BadRequest));


            try
            {
                var domName= ActiveDirectoryService.Get_correct_DomainName(login.domain);
                ///Virifica contra domino
                var res = ActiveDirectoryService.User_Logon(login.username, login.password, domName);
                //var resp = apiHelper.fromObject<LoogonUserResult>(res);

                if (res.Autenticated == false)
                {
                    //return Unauthorized();
                    return apiHelper.fromErrorString("No esta autorizado para ingresar a este dominio", HttpStatusCode.Unauthorized );
                }
                //si la verificacion contra dominio es OK
                //busco info del dmonio 
                int dom_id = MeucciDAC.GetDimainId(login.domain);
                //verifico en meucci el usuario : si es que tiene permiso y no esta ausente este dia: 
                 var emmpleadoBE =  MeucciDAC.VirifyUser(login.username, dom_id);
         
                emmpleadoBE.Dominio = login.domain;

                var token = TokenGenerator.GenerateTokenJwt(emmpleadoBE);
                //return Ok(token);
                return apiHelper.fromObject<string>(token);
                
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
                //return new  System.Web.Http.Results.ExceptionResult(ex,this);
            }


           
        }
    }
}
