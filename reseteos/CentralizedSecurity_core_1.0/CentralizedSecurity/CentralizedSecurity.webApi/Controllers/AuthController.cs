using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CentralizedSecurity.webApi.common;
using CentralizedSecurity.webApi.DAC;
using CentralizedSecurity.webApi.helpers;
using CentralizedSecurity.webApi.Models;
using Fwk.CentralizedSecurity.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedSecurity.webApi.Controllers
{
    [Route("/api/oauth")]
    [ApiController]
    public class OAuthController : Controller
    {

        private readonly IMeucciService meucciService;
        public OAuthController(IMeucciService service)
        {
            meucciService = service;
        }
        [HttpGet("ping")]
        [AllowAnonymous]
        public IActionResult ping()
        {
            return Ok("El servicio funciona correctamente /api/oauth");
        }

        /// <summary>
        /// Api de autenticacion que genera el jwt.- Realiza validaciones contra meucci y el dominio espesificado.
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Token tjw</returns>
        [HttpPost()]
        [Route("authenticate")]
        [AllowAnonymous]
        public  IActionResult Authenticate(LoginRequest login)
         {
            if (login == null)
             return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "El parametro login es requerido"));

            if (string.IsNullOrEmpty(login.username))
              return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "El parametro username es requerido"));


            try
            {
                //var token = TokenGenerator.GenerateTokenJwt(emmpleadoBE);
               var  token = meucciService.Authenticate(login.username, login.password, login.domain);
                return Ok(token);
            }
            catch (Fwk.Exceptions.FunctionalException fx)
            {
                var status = Enum.Parse<HttpStatusCode>(fx.ErrorId);
                return BadRequest(new ApiErrorResponse(status, fx.Message));
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }



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
            ActiveDirectoryUser user = null;
            if (login == null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Los parámetros del loging no son opcionales"));
            try
            {
                var res = ActiveDirectoryService.User_Logon(login.username, login.password, login.domain);

                if (res.Autenticated)
                {
                    if (login.includeDomainUserData)
                    {
                        try
                        {
                            user = ActiveDirectoryService.User_Info(login.username, login.domain);
                        }
                        catch (Exception ex)
                        {
                            res.ErrorMessage = "No fué posible obtener datos del usuario en el dominio. Razon =  " + ex.Message;
                        }

                    }


                    List<ActiveDirectoryGroup> userGroups = null;
                    if (login.includeGroups)
                    {

                        try
                        {
                            userGroups = ActiveDirectoryService.GetGroupsFromUser(login.username, login.domain);
                        }
                        catch (Exception ex)
                        {
                            res.ErrorMessage = "No fué posible obtener los grupos usuario en el dominio. Razon =  " + ex.Message;
                        }
                    }


                    var jwt = TokenGenerator.GenerateTokenJwt_LDAP(login.username, user, userGroups);

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
            ActiveDirectoryUser user = null;
            if (login == null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "Los parámetros del loging no son opcionales"));
            try
            {
                //var res = ActiveDirectoryService.User_Logon(login.username, login.password, login.domain);
                var res = new LoogonUserResult();
                res.Autenticated = true;
                if (res.Autenticated)
                {
                    if (login.includeDomainUserData)
                    {
                        try
                        {
                            //user = ActiveDirectoryService.User_Info(login.username, login.domain);
                            user = new ActiveDirectoryUser();
                            user.Company = "contoso";
                            user.Country = "arg";
                            user.FirstName = login.username;
                            user.LoginName = login.username;
                            user.EmailAddress = login.username + "@contoso.co";
                        }
                        catch (Exception ex)
                        {
                            res.ErrorMessage = "No fué posible obtener datos del usuario en el dominio. Razon =  " + ex.Message;
                        }

                    }


                    List<ActiveDirectoryGroup> userGroups = null;
                    if (login.includeGroups)
                    {

                        try
                        {
                            

                            userGroups = new List<ActiveDirectoryGroup>();
                            ActiveDirectoryGroup g = new ActiveDirectoryGroup();
                            g.CN = "co";
                            g.Description = "co";
                            g.Name = "contoso";
                            userGroups.Add(g);
                        }
                        catch (Exception ex)
                        {
                            res.ErrorMessage = "No fué posible obtener los grupos usuario en el dominio. Razon =  " + ex.Message;
                        }
                    }


                    var jwt = TokenGenerator.GenerateTokenJwt_test(login.username, user, userGroups);

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