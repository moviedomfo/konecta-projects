using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CentralizedSecurity.webApi.Models;
using Fwk.CentralizedSecurity.helpers;
using CentralizedSecurity.webApi.helpers;
using CentralizedSecurity.webApi.DAC;

namespace CentralizedSecurity.webApi.Controllers
{
    //http://localhost:50010/Swagger/ui/index#/api

        /// <summary>
        /// Apis de meucci
        /// </summary>
    [JWTAuthentication]
    [RoutePrefix("api/meucci")]
    public class MeucciController : ApiController
    {

        public HttpResponseMessage Get()
        {
            
            return apiHelper.fromObject<string>("api/meucci");
        }

        /// <summary>
        /// Api de autenticacion que genera el jwt.- Realiza validaciones contra meucci y el dominio
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
                    return apiHelper.fromErrorString("Usuario no registrado en Meucci", HttpStatusCode.Unauthorized);
                }

                if (string.IsNullOrEmpty(emmpleadoBE.Cuenta) && emmpleadoBE.CAIS == false)
                {
                    return apiHelper.fromErrorString("Usuario no habilitado ", HttpStatusCode.Unauthorized);
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
        /// Verifica si existe un usuario en un dominiopor medio de su dni
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Retorna todos los usuarios relacionados al dni de todos los dominios</returns>
        [HttpPost]
        [Route("retriveEmpleadosReseteos")]
        public HttpResponseMessage retriveEmpleadosReseteos(retriveEmpleadosReseteosReq req)//string userName, string domain, string dni)
        {
            try
            {
                int domain_id = DAC.MeucciDAC.GetDimainId(req.domain);
                var empleado = DAC.MeucciDAC.RetriveDatosReseteoEmpleados(req.userName, domain_id, req.dni);

                if (empleado == null || empleado.WindosUserList == null)
                {
                    //a.Si el usuario pertenece al CAIS
                    if (req.userCAIS)
                    {
                        throw new Fwk.Exceptions.FunctionalException("El DNI ingresado no existe");
                    }
                    else
                    {
                        //b.Si el usuario no pertenece al CAIS se deberá mostrar el mensaje 
                        throw new Fwk.Exceptions.FunctionalException("El DNI ingresado no existe o no tiene visibilidad sobre el mismo. Por favor comuníquese con el CAIS.");
                    }
                }


                //empleado <> NULL
                // Si Aus_Id un valor distinto de NULL, 
                if (empleado.Aus_Id.HasValue)
                {
                    //2.Si la ejecución del SP devuelve en el campo Aus_Id un valor distinto de NULL, sin importar si el usuario pertenece o no al CAIS se deberá mostrar el mensaje 
                    throw new Fwk.Exceptions.FunctionalException("No es posible resetear o desbloquear " +
                        "el Usuario de Windows porque el empleado se encuentra ausente. Ante la duda, " +
                        "comuníquese con el ACI");
                }



                var resp = apiHelper.fromObject<EmpleadoBE>(empleado);
                return resp;
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }

        /// <summary>
        /// Retorna dominios almacenados en la BD de Meucci 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("retriveDomains")]
        public HttpResponseMessage retriveDomains()//string userName, string domain, string dni)
        {
            try
            {
                var res = DAC.MeucciDAC.RetriveDommains();

                var resp = apiHelper.fromObject<List<DomainsBE>>(res);
                return resp;
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }




        /// <summary>
        /// Metodo solo para test.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public HttpResponseMessage ping()
        {
            return apiHelper.fromObject<String>("El servicio funciona correctamente");
        }


        /// <summary>
        /// Retorna informacion del servidor de reseteos web-api
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("getServerInfo")]
        public HttpResponseMessage GetServerInfo()
        {
            try
            {

                ApiServerInfo info = new ApiServerInfo();
                var sql = Common.Get_SqlConnectionStringBuilder(Common.CnnStringNameMeucci);
                info.SQLServerMeucci = "Coneccion :" + Common.CnnStringNameMeucci + " Sql Server = " + sql.DataSource + " BD " + sql.InitialCatalog;

                sql = Common.Get_SqlConnectionStringBuilder(Common.CnnStringNameAD);
                info.SQLServerMeucci = "Coneccion :" + Common.CnnStringNameAD + " Sql Server = " + sql.DataSource + " BD " + sql.InitialCatalog;

                info.Ip = Common.Get_IPAddress();
                info.HostName = Dns.GetHostName();
                return apiHelper.fromObject<ApiServerInfo>(info);
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }



    }
}