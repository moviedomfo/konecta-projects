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
using Fwk.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CentralizedSecurity.webApi.Controllers
{
    [Route("/api/ldap")]
    [ApiController]
    public class ActiveDirectoryController : ControllerBase
    {
        private ILDAPService _LDAPService;

        public ActiveDirectoryController(ILDAPService _lDAPService)
        {


        }

        /// <summary>
        /// Permite hacer un ping al servicio para determinar si esta activo, Método solo para test.
        /// </summary>
        /// <returns>Retorna un mensaje si esta activo</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public IActionResult ping()
        {
            return Ok("El servicio funciona correctamente");
        }



        /// <summary>
        /// Api para autenticar usuario. No genera jwt solo actua contra active directory
        /// </summary>
        /// <param name="login"></param>
        /// <returns>retorna <see cref="LoogonUserResult"/> </returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult Authenticate(LoginRequest login)
        {
            if (login == null)
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, "LoginRequest no puede ser nulo"));
            try
            {
                var res = ActiveDirectoryService.User_Logon(login.username, login.password, login.domain);
               
                return Ok(res);
            }
            catch (Exception ex)
            {
                //return apiHelper.fromEx(ex);
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }

        }




        ///  <summary>
        /// Verifica si existe un usuario en un dominio   
        /// </summary>
        /// <param name="userName">Nombre de usuario a buscar</param>
        /// <param name="domain">Dominio ej: allus.ar, alcomovistar</param>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult userExist(string userName, string domain)
        {
            try
            {
                var res = ActiveDirectoryService.UserExist(userName, domain);
                return Ok(res);
                
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }

        /// <summary>
        /// Retorna datos de un usuario en un dominio determinado
        /// </summary>
        /// <param name="userName">Nombre de usuario a buscar</param>
        /// <param name="domain">Dominio ej: allus.ar, alcomovistar</param>
        [HttpGet]
        [Route("retriveUserByName")]
        public IActionResult retriveUserByName(string userName, string domain)
        {
            try
            {

                ActiveDirectoryUser res = ActiveDirectoryService.User_Info(userName, domain);

                return Ok(res);
                //return apiHelper.fromObject<ActiveDirectoryUser>(res);


            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }


        /// <summary>
        /// Retorna todos los grupos de un determinado usuario/dominio
        /// </summary>
        /// <param name="userName">Nombre de usuario a buscar</param>
        /// <param name="domain">Dominio ej: allus.ar, alcomovistar</param>
        /// <returns></returns>
        [HttpGet]
        [Route("retriveUserGroups")]
        public IActionResult retriveUserGroups(string userName, string domain)
        {
            try
            {
                var list = ActiveDirectoryService.GetGroupsFromUser(userName, domain);
                if (list == null)
                    //return apiHelper.fromErrorString(string.Format("No se encontraron grupos para el usuario {0} y dominio {1}", userName, domain), HttpStatusCode.NoContent);
                    return BadRequest(string.Format("No se encontraron grupos para el usuario {0} y dominio {1}", userName, domain));
                else
                    return Ok(list);
                //return apiHelper.fromObject<IEnumerable<ActiveDirectoryGroup>>(list);
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }

        /// <summary>
        /// Retorna todos los usuarios de un determinado Grupo/Dominio
        /// </summary>
        /// <param name="groupName">Nombre de grupo a buscar</param>
        /// <param name="domain">Dominio ej: allus.ar, alcomovistar</param>
        /// <returns></returns>
        [HttpGet]
        [Route("retriveGroupUsers")]
        public IActionResult retriveGroupUsers(string groupName, string domain)
        {
            try
            {
                var list = ActiveDirectoryService.GetUsersFromGroup(groupName, domain);
                if (list == null)
                    return BadRequest(string.Format("No se encontraron usuarios para este grupo {0} y dominio {1}", groupName, domain));
                    //return apiHelper.fromErrorString(string.Format("No se encontraron usuarios para este grupo {0} y dominio {1}", groupName, domain), HttpStatusCode.NoContent);
                else

                     return Ok(list);
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }

        /// <summary>
        /// Retorna todos los grupos de un determinado Dominio
        /// </summary>
        /// <param name="domain">Dominio ej: allus.ar, alcomovistar</param>
        /// <returns></returns>
        [HttpGet]
        [Route("retriveGroups")]
        // Retorna todos los grupos de un determinado dominio
        public IActionResult retriveGroups(string domain)
        {

            try
            {
                var list = ActiveDirectoryService.GetGroups(domain);
                if (list == null)
                    //return apiHelper.fromErrorString(string.Format("No se grupos para el dominio {0}", domain), HttpStatusCode.NoContent);
                    return BadRequest(string.Format("No se grupos para el dominio {0}", domain));
                else
                    return Ok(list);
            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }


        /// <summary>
        /// Permite reseteo de de clave de usuaria.
        /// </summary>
        /// <param name="req">Parametros de entrada</param>
        /// <returns>retrna  la clave generada aleatoriamente</returns>
        [HttpPost]
        [Route("userResetPassword")]
        public IActionResult userResetPassword(userResetPasswordReq req)
        {

            try
            {

                //int domain_id = DAC.MeucciDAC.GetDimainId(req.DomainName);

                if (req.ResetUserCAIS == false)
                {

                    //Regla Si se intenta resetear por tercera o más veces un UW el mismo día, 
                    var intentos = MeucciDAC.ValidarteIntentos(req.WindowsUser, req.dom_id, Common.resetear);
                    if (intentos > 2)
                    {
                        throw new Fwk.Exceptions.FunctionalException("No es posible resetear más de dos veces el Usuario Windows el mismo día. Por favor comuníquese con el CAIS.");
                    }
                }
                //Si el usuario pertenece al CAIS será obligatoria la carga del Nº de Ticket.
                if (req.ResetUserCAIS)
                {
                    if (string.IsNullOrEmpty(req.ticket))
                    {
                        throw new Fwk.Exceptions.FunctionalException("El Nº de Ticket es obligatorio .-");
                    }
                }
                req.newPassword = _LDAPService.GetRandomPassword();

                var fwk_domain_name = _LDAPService.Get_correct_DomainName(req.DomainName);

                ActiveDirectoryService.User_Reset_Password(req.WindowsUser, req.newPassword, fwk_domain_name);

                DAC.MeucciDAC.ReseteoWeb_Log(req.Emp_Id, req.WindowsUser, req.dom_id, req.ResetUserId, req.ticket, Common.resetear, req.host);
                DAC.MeucciDAC.ReseteoWeb_EnviosMails(req.Emp_Id, req.WindowsUser, req.dom_id, req.ResetUserId, Common.resetear, req.host);
                //return apiHelper.fromObject<String>("El reseteo se realizó exitosamente. La contraseña provisoria es " + req.newPassword);

                return Ok("El reseteo se realizó exitosamente. La contraseña provisoria es " + req.newPassword);
            }
            catch (Exception ex)
            {
                //Acceso denegado. (Excepción de HRESULT: 0x80070005 (E_ACCESSDENIED))
                var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                if (msg.Contains("E_ACCESSDENIED"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.WindowsUser + " en el dominio " + req.DomainName
                        + " deberá comunicarce con segurridad informática ", ex);
                    ex = t;
                }

                msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }

        /// <summary>
        /// Permite desbloquear un usuario de windows contra su dominio
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("userUnlock")]
        public IActionResult User_Unlock(userUnlockReq req)
        {


            try
            {
                if (req.ResetUserCAIS == false)
                {

                    //regla se intenta desbloquear por tercera vez un UW el mismo día
                    var intentos = MeucciDAC.ValidarteIntentos(req.WindowsUser, req.dom_id, Common.resetear);
                    if (intentos > 2)
                    {
                        throw new Fwk.Exceptions.FunctionalException("No es posible desbloquear más de dos veces el Usuario Windows el mismo día. Por favor comuníquese con el CAIS.");
                    }
                }
                //Si el usuario pertenece al CAIS será obligatoria la carga del Nº de Ticket.
                if (req.ResetUserCAIS)
                {
                    if (String.IsNullOrEmpty(req.ticket))
                    {
                        throw new Fwk.Exceptions.FunctionalException("El Nº de Ticket es obligatorio .-");
                    }
                }
                var fwk_domain_name = _LDAPService.Get_correct_DomainName(req.DomainName);
                ActiveDirectoryService.User_Unlock(req.WindowsUser, fwk_domain_name);
                DAC.MeucciDAC.ReseteoWeb_Log(req.emp_id, req.WindowsUser, req.dom_id, req.ResetUserId, req.ticket, Common.desbloquear, req.host);
                DAC.MeucciDAC.ReseteoWeb_EnviosMails(req.emp_id, req.WindowsUser, req.dom_id, req.ResetUserId, Common.desbloquear, req.host);
                return Ok("El desbloqueo se realizó correctamente");

            }
            catch (Exception ex)
            {
                var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                if (msg.Contains("E_ACCESSDENIED"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.WindowsUser + " en el dominio " + req.DomainName
                        + " deberá comunicarce con segurridad informática ", ex);
                    ex = t;
                }
                 msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg));
            }
        }


      

    }
}
