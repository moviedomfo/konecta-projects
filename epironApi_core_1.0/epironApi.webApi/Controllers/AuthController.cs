using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using epironApi.webApi.common;
using epironApi.webApi.DAC;
using epironApi.webApi.helpers;
using epironApi.webApi.Models;
using Fwk.epironApi.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace epironApi.webApi.Controllers
{
    [Route("/api/oauth")]
    [ApiController]
    public class OAuthController : Controller
    {

        private readonly IEpironService epironService;
        public OAuthController(IEpironService service)
        {
            epironService = service;
        }
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult ping()
        {
            return Ok("El servicio funciona correctamente /api/oauth");
        }

    
        /// <summary>
        /// Api de autenticacion que genera el jwt.- Realiza validaciones solo contra el dominio espesificado. 
        /// </summary>
        /// <param name="login"></param>
        /// <returns>jwt</returns>
        [AllowAnonymous]
        [HttpPost("auth")]
        public IActionResult auth(LoginRequestAuth login)
        {
            
            if (login == null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Los parámetros del loging no son opcionales"));
            try
            {
                var res = new LoginRequestAuthRes(); //ActiveDirectoryService.User_Logon(login.username, login.password, login.domain);

                if (res.Autenticated)
                {






                    UserAPiBE emp = new UserAPiBE();

                    var jwt = TokenGenerator.GenerateTokenEpiron(emp);

                    res.Token = jwt;
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }

        }



        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult authTest(LoginRequestAuth login)
        {
            LoginRequestAuthRes res = new  LoginRequestAuthRes();
        
            if (login == null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Los parámetros del loging no son opcionales"));
            try
            {
                //var res = ActiveDirectoryService.User_Logon(login.username, login.password, login.domain);
                
                res.Autenticated = true;
                if (res.Autenticated)
                {
                    if (login.includeDomainUserData)
                    {
                        try
                        {
                            //user = ActiveDirectoryService.User_Info(login.username, login.domain);
                            res = new LoginRequestAuthRes();
                        }
                        catch (Exception ex)
                        {
                            res.ErrorMessage = "No fué posible obtener datos del usuario en el dominio. Razon =  " + ex.Message;
                        }

                    }


                   


                    var jwt = TokenGenerator.GenerateTokenJwt_test(login.username);

                    res.Token = jwt;
                }

                return Ok(res);

            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }

        }

    }
}