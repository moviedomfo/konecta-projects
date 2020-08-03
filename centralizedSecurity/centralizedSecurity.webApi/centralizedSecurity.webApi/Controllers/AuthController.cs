using System;
using System.Net;
using System.Web.Http;
using CentralizedSecurity.webApi.Models;
using Fwk.CentralizedSecurity.helpers;
using CentralizedSecurity.webApi.DAC;
using System.Net.Http;
using Fwk.CentralizedSecurity.Contracts;
using System.Collections.Generic;
using Microsoft.Owin.Security.DataHandler.Encoder;
using System.Security.Cryptography;
using System.IdentityModel.Tokens;

namespace CentralizedSecurity.webApi.Controllers
{
    /// <summary>
    /// Autenticacion JWT
    /// </summary>
    [AllowAnonymous]
    [RoutePrefix("api/oauth")]
    public class AuthController : ApiController
    {


        /// <summary>
        /// Api de autenticacion que genera el jwt.- Realiza validaciones contra meucci y el dominio
        /// Esta api solo la usa la web de reseteos. La misma se encuentra en el controlador Meucci
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Token tjw</returns>
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
                var domName = ActiveDirectoryService.Get_correct_DomainName(login.domain);
                ///Virifica contra domino
                var res = ActiveDirectoryService.User_Logon(login.username, login.password, domName);
                //var resp = apiHelper.fromObject<LoogonUserResult>(res);

                if (res.LogResult == "LOGIN_USER_OR_PASSWORD_INCORRECT")
                //    if (res.Autenticated == false)
                {
                    //return Unauthorized();
                    return apiHelper.fromErrorString("El usuario y/o contraseña es incorrecto ", HttpStatusCode.Unauthorized);
                }

                if (res.LogResult == "LOGIN_USER_DOESNT_EXIST")
                {
                    return apiHelper.fromErrorString("El usuario no existe en el dominio  " + login.domain, HttpStatusCode.Unauthorized);
                }

                //si la verificacion contra dominio es OK
                //busco info del dmonio 
                int dom_id = MeucciDAC.GetDimainId(login.domain);




                var emmpleadoBE = MeucciDAC.VirifyUser(login.username, dom_id);

                //Emp_Id, legajo correspondiente al usuario reseteador, si devuelve NULL mostrar el mensaje “Usuario no registrado en Meucci” y cerrar aplicación.
                //o Cue_Id, cuenta correspondiente al usuario reseteador, si devuelve NULL y el campo CAIS es 0, mostrar el mensaje “Usuario no habilitado” 

                if (emmpleadoBE == null)
                {
                    emmpleadoBE = new EmpleadoReseteoBE();
                    emmpleadoBE.Emp_id = -1;
                    emmpleadoBE.WindowsUser = login.username ;
                    emmpleadoBE.Legajo = -1;
                    emmpleadoBE.Legajo = -1;
                    emmpleadoBE.CAIS = false;
                    emmpleadoBE.isRessetUser = false;
                    emmpleadoBE.Cuenta = "";
                    emmpleadoBE.Cargo = "";

                    //return apiHelper.fromErrorString("Usuario no registrado en Meucci", HttpStatusCode.Unauthorized);
                }

                if (string.IsNullOrEmpty(emmpleadoBE.Cuenta) && emmpleadoBE.CAIS == false)
                {
                    emmpleadoBE.isRessetUser = false;
                    //return apiHelper.fromErrorString("Usuario no habilitado ", HttpStatusCode.Unauthorized);
                }
                else
                {
                   emmpleadoBE.isRessetUser = true;
                }

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


        /// <summary>
        /// Api de autenticacion que genera el jwt.- Realiza validaciones el dominio espesificado. 
        /// </summary>
        /// <param name="login"></param>
        /// <returns><see cref="LoogonUserResult"/></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("auth")]
        public HttpResponseMessage auth(LoginRequestAuth login)
        {
            ActiveDirectoryUser user = null;
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            try
            {
                var domName = ActiveDirectoryService.Get_correct_DomainName(login.domain);

                var res = ActiveDirectoryService.User_Logon(login.username, login.password, domName);

                if (res.Autenticated)
                {
                    if (login.includeDomainUserData)
                    {
                        try
                        {
                            user = ActiveDirectoryService.User_Info(login.username, domName);
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
                            userGroups = ActiveDirectoryService.GetGroupsFromUser(login.username, domName);
                        }
                        catch (Exception ex)
                        {
                            res.ErrorMessage = "No fué posible obtener los grupos usuario en el dominio. Razon =  " + ex.Message;
                        }
                    }


                    var jwt = TokenGenerator.GenerateTokenJwt_LDAP(login.username, user, userGroups);

                    res.Token = jwt;
                }

                var resp = apiHelper.fromObject<LoogonUserResult>(res);


                return resp;
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }

        }





        [AllowAnonymous]
        [HttpGet]
        [Route("genJWTKey")]
        public HttpResponseMessage genJWTKey()
        {
            
            //var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainTextkey);
            //var res = System.Convert.ToBase64String(plainTextBytes);
            

            var key = new byte[32];
            RNGCryptoServiceProvider.Create().GetBytes(key);
            var base64Secret = TextEncodings.Base64Url.Encode(key);
            var resp = apiHelper.fromObject(base64Secret);
            return resp;


        }
        [AllowAnonymous]
        [HttpGet]
        [Route("resolveJWTKey")]
        public HttpResponseMessage resolveJWTKey(string base64EncodedData)
        {

            var symmetricKeyAsBase64 = base64EncodedData;
            var keyByteArray = TextEncodings.Base64Url.Decode(symmetricKeyAsBase64);

            var resp = apiHelper.fromObject("Valid RSA base64EncodedData");


            return resp;


        }
    }
}
