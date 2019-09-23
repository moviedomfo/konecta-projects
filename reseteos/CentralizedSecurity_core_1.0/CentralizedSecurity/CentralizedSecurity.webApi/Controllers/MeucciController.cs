using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using CentralizedSecurity.webApi.common;
using CentralizedSecurity.webApi.helpers;
using CentralizedSecurity.webApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedSecurity.webApi.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
        [ApiController]
    public class MeucciController : ControllerBase
    {

        private ILDAPService lDAPService;
        private IMeucciService meucciService;
  
        public MeucciController(IMeucciService meucciService, ILDAPService _lDAPService)
        {


        }
        /// <summary>
        /// Metodo solo para test.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult pingSecure()
        {
            return Ok("El ping secure funciona correctamente");
        }

        /// <summary>
        /// Metodo solo para test.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult ping()
        {
            return Ok("El servicio funciona correctamente");
        }

        /// <summary>
        /// Api de autenticacion que genera el jwt.- Realiza validaciones contra meucci y el dominio
        /// </summary>
        /// <param name="login"></param>
        /// <returns>Token tjw</returns>
        [HttpPost("[action]")]
        public IActionResult authenticate(LoginRequest login)
        {
            if (login == null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "El parametro login es requerido"));

            if (string.IsNullOrEmpty(login.username))
                return BadRequest(new ApiErrorResponse(HttpStatusCode.BadRequest, "El parametro username es requerido"));

            try
            {
                var token = meucciService.Authenticate(login.username, login.password, login.domain);
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
        /// Verifica si existe un usuario en un dominiopor medio de su dni
        /// </summary>
        /// <param name="req"></param>
        /// <returns>Retorna todos los usuarios relacionados al dni de todos los dominios</returns>

        [HttpPost("[action]")]
        public IActionResult retriveEmpleadosReseteos(retriveEmpleadosReseteosReq req)//string userName, string domain, string dni)
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



                var resp = Ok(empleado);
                return resp;
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }

        /// <summary>
        /// Retorna dominios almacenados en la BD de Meucci 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult retriveDomains()//string userName, string domain, string dni)
        {
            try
            {
                var res = DAC.MeucciDAC.RetriveDommains();

                var resp = Ok(res);
                return resp;
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }




     

        /// <summary>
        /// Retorna informacion del servidor de reseteos web-api
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult GetServerInfo()
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
                return Ok(info);
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg)); ;
            }
        }


    }
}