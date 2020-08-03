using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CentralizedSecurity.webApi.Models;
using Fwk.CentralizedSecurity.helpers;
using Fwk.CentralizedSecurity.Contracts;
using CentralizedSecurity.webApi.helpers;
using CentralizedSecurity.webApi.DAC;
using CentralizedSecurity.webApi.service;
using Fwk.Exceptions;

namespace CentralizedSecurity.webApi.Controllers
{
    /// <summary>
    /// API de autenticacion ldap
    /// </summary>
    [JWTAuthentication]
    [RoutePrefix("api/ldap")]
    public class ActiveDirectoryController : ApiController
    {
        
        ILDAPService _LDAPService ;

        /// <summary>
        /// API de autenticacion ldap
        /// </summary>
        public ActiveDirectoryController()
        {
            _LDAPService =  LDAPService.CreateInstance();
        }

        /// <summary>
        /// Api para autenticar usuario. No genera jwt solo actua contra active directory
        /// </summary>
        /// <param name="login"></param>
        /// <returns>retorna <see cref="LoogonUserResult"/> </returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("authenticate")]
        public HttpResponseMessage Authenticate(LoginRequest login)
        {
            if (login == null)
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            try
            {
                var res = ActiveDirectoryService.User_Logon(login.username, login.password, login.domain);
                var resp = apiHelper.fromObject<LoogonUserResult>(res);
                return resp;
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }

        }



        ///  <summary>
        /// Verifica si existe un usuario en un dominio   
        /// </summary>
        /// <param name="userName">Nombre de usuario a buscar</param>
        /// <param name="domain">Dominio ej: allus.ar, alcomovistar</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("userExist")]
        public HttpResponseMessage userExist(string userName, string domain)
        {
            try
            {
                var res = ActiveDirectoryService.UserExist(userName, domain);
                var resp = apiHelper.fromObject<Boolean>(res);
                return resp;
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }

        /// <summary>
        /// Retorna datos de un usuario en un dominio determinado
        /// </summary>
        /// <param name="userName">Nombre de usuario a buscar</param>
        /// <param name="domain">Dominio ej: allus.ar, alcomovistar</param>
        [HttpGet]
        [Route("retriveUserByName")]
        public HttpResponseMessage retriveUserByName(string userName, string domain)
        {
            try
            {

                ActiveDirectoryUser res = ActiveDirectoryService.User_Info(userName, domain);

                return apiHelper.fromObject<ActiveDirectoryUser>(res);


            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
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
        public HttpResponseMessage retriveUserGroups(string userName, string domain)
        {
            try
            {
                var list = ActiveDirectoryService.GetGroupsFromUser(userName, domain);
                if (list == null)
                    return apiHelper.fromErrorString(string.Format("No se encontraron grupos para el usuario {0} y dominio {1}", userName, domain), HttpStatusCode.NoContent);
                else
                    return apiHelper.fromObject<IEnumerable<ActiveDirectoryGroup>>(list);
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
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
        public HttpResponseMessage retriveGroupUsers(string groupName, string domain)
        {
            try
            {
                var list = ActiveDirectoryService.GetUsersFromGroup(groupName, domain);
                if (list == null)
                    return apiHelper.fromErrorString(string.Format("No se encontraron usuarios para este grupo {0} y dominio {1}", groupName, domain), HttpStatusCode.NoContent);
                else

                    return apiHelper.fromObject<List<ActiveDirectoryUser>>(list);
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
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
        public HttpResponseMessage retriveGroups(string domain)
        {

            try
            {
                var list = ActiveDirectoryService.GetGroups(domain);
                if (list == null)
                    return apiHelper.fromErrorString(string.Format("No se grupos para el dominio {0}", domain), HttpStatusCode.NoContent);
                else

                    return apiHelper.fromObject<List<ActiveDirectoryGroup>>(list);
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }


        /// <summary>
        /// Retorna informacion sobre dominios de la empresa de la BD de seguridad
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("retriveDomainsUrl")]
        public HttpResponseMessage retriveDomainsUrl()
        {

            try
            {
                
                var list = ActiveDirectoryService.GetAllDomainsUrl();
                if (list == null)
                    return apiHelper.fromErrorString("No se encontaron los DomainsUrl configurados en la BD", HttpStatusCode.NoContent);
                else
                    return apiHelper.fromObject<List<DomainsUrl>>(list.ToList());
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }


        /// <summary>
        /// Permite reseteo de de clave de usuaria.
        /// </summary>
        /// <param name="req">Parametros de entrada</param>
        /// <returns>retrna  la clave generada aleatoriamente</returns>
        [HttpPost]
        [Route("userResetPassword")]
        public HttpResponseMessage userResetPassword(userResetPasswordReq req)
        {

            try
            {

                //int domain_id = DAC.MeucciDAC.GetDimainId(req.DomainName);

                if (req.ResetUserCAIS == false)
                {
                   
                    //Regla Si se intenta resetear por tercera o más veces un UW el mismo día, 
                    var intentos = MeucciDAC.ValidarteIntentos(req.WindowsUser, req.dom_id, Common.resetear);
                    if (intentos >= 2)
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
                //ActiveDirectoryService.User_MustChangePasswordNextLogon(req.WindowsUser,true, fwk_domain_name);
                 DAC.MeucciDAC.ReseteoWeb_Log(req.Emp_Id, req.WindowsUser, req.dom_id, req.ResetUserId, req.ticket, Common.resetear, req.host);
                DAC.MeucciDAC.ReseteoWeb_EnviosMails(req.Emp_Id, req.WindowsUser, req.dom_id, req.ResetUserId,  Common.resetear, req.host);
                return apiHelper.fromObject<String>("El reseteo se realizó exitosamente. La contraseña provisoria es " + req.newPassword);
            }
            catch (Exception ex)
            {

                var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                if (msg.ToLower().Contains("impersonali") || msg.ToLower().Contains("acceso"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.WindowsUser + " en el dominio " + req.DomainName
                        + " deberá comunicarse  con CAIS ");
                    ex = t;
                }
                if (msg.Contains("E_ACCESSDENIED"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.WindowsUser + " en el dominio " + req.DomainName
                        + " deberá comunicarse con CAIS ");
                    ex = t;
                }
                return apiHelper.fromEx(ex);
            }
        }


        /// <summary>
        ///  
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [Route("userMustChangePasswordNextLogon")]
        public HttpResponseMessage userMustChangePasswordNextLogon(MustChangePasswordNextLogonReq req)
        {

            try
            {

                var fwk_domain_name = _LDAPService.Get_correct_DomainName(req.domain);

                
                ActiveDirectoryService.User_MustChangePasswordNextLogon(req.userName, true, fwk_domain_name);
                
                return apiHelper.fromObject<String>("La operacion de solicitud de cambio de contraseña en proximo loging se realizó exitosamente.");
            }
            catch (Exception ex)
            {

                var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                if (msg.ToLower().Contains("impersonali") || msg.ToLower().Contains("acceso"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.userName + " en el dominio " + req.domain
                        + " deberá comunicarse  con CAIS ");
                    ex = t;
                }
                if (msg.Contains("E_ACCESSDENIED"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.userName + " en el dominio " + req.domain
                        + " deberá comunicarse con CAIS ");
                    ex = t;
                }
                return apiHelper.fromEx(ex);
            }
        }

        /// <summary>
        /// Permite desbloquear un usuario de windows contra su dominio
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("userUnlock")]
        public HttpResponseMessage User_Unlock(userUnlockReq req)
        {


            try
            {
                if (req.ResetUserCAIS == false)
                {
                    
                    //regla se intenta desbloquear por tercera vez un UW el mismo día
                    var intentos = MeucciDAC.ValidarteIntentos(req.WindowsUser, req.dom_id, Common.desbloquear);
                    if (intentos >= 2)
                    {
                        throw new Fwk.Exceptions.FunctionalException("No es posible desbloquear más de dos veces el Usuario Windows el mismo día. Por favor comuníquese con el CAIS.");
                    }
                }
                //Si el usuario pertenece al CAIS será obligatoria la carga del Nº de Ticket.
                if (req.ResetUserCAIS)
                {
                    if (String.IsNullOrEmpty(req.ticket) )
                    {
                        throw new Fwk.Exceptions.FunctionalException("El Nº de Ticket es obligatorio .-");
                    }
                }
                var fwk_domain_name = _LDAPService.Get_correct_DomainName(req.DomainName);
                ActiveDirectoryService.User_Unlock(req.WindowsUser, fwk_domain_name);
                DAC.MeucciDAC.ReseteoWeb_Log(req.emp_id, req.WindowsUser, req.dom_id, req.ResetUserId, req.ticket, Common.desbloquear, req.host);
                DAC.MeucciDAC.ReseteoWeb_EnviosMails(req.emp_id, req.WindowsUser, req.dom_id, req.ResetUserId,  Common.desbloquear, req.host);
                return apiHelper.fromObject<string>("El desbloqueo se realizó correctamente");
            }
            catch (Exception ex)
            {

                var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                if (msg.ToLower().Contains("impersonali") || msg.ToLower().Contains("acceso"))
                {
                    TechnicalException t = new TechnicalException("No es posible desbloquear " + req.WindowsUser + " en el dominio " + req.DomainName
                        + " deberá comunicarse  con el CAIS");
                    ex = t;
                }
                if (msg.Contains("E_ACCESSDENIED"))
                {
                    TechnicalException t = new TechnicalException("No es posible desbloquear " + req.WindowsUser + " en el dominio " + req.DomainName
                        + " deberá comunicarse con el CAIS");
                    ex = t;
                }

                return apiHelper.fromEx(ex);
            }
        }


        /// <summary>
        /// Permite cambio de  de de clave de usuari.
        /// </summary>
        /// <param name="req">Parametros de entrada</param>
        /// <returns>ok or error</returns>
        [HttpPost]
        [Route("userChangePassword")]
        public HttpResponseMessage userChangePassword(userChangePasswordReq req)
        {

            try
            {
                var fwk_domain_name = _LDAPService.Get_correct_DomainName(req.domainName);

                ActiveDirectoryService.User_Reset_Password(req.userName, req.newPassword, fwk_domain_name);
                
                DAC.MeucciDAC.ReseteoWeb_Log(req.emp_Id, req.userName, req.dom_id, req.userId,"" , Common.changePwd, req.host);
                //DAC.MeucciDAC.ReseteoWeb_EnviosMails(req.emp_Id, req.WindowsUser, req.dom_id, req.ResetUserId, Common.resetear, req.host);
                return apiHelper.fromObject<String>("La contraseña se cambio exitosamente. ");
            }
            catch (Exception ex)
            {

                var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                if (msg.ToLower().Contains("impersonali") || msg.ToLower().Contains("acceso"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.userName + " en el dominio " + req.domainName
                        + " deberá comunicarse  con CAIS ");
                    ex = t;
                }
                if (msg.Contains("E_ACCESSDENIED"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.userName + " en el dominio " + req.domainName
                        + " deberá comunicarse con CAIS ");
                    ex = t;
                }
                return apiHelper.fromEx(ex);
            }
        }


        [HttpPost]
        [Route("userChangePasswordSelf")]
        [AllowAnonymous]
        public HttpResponseMessage userChangePasswordSelf(userChangePasswordReq req)
        {

            try
            {
                var intentos = MeucciDAC.ValidarteIntentos(req.userName, req.dom_id, Common.changePwdSelft);
                if (intentos >= 2)
                {
                    throw new Fwk.Exceptions.FunctionalException("No es posible resetear más de dos veces el Usuario Windows el mismo día. Por favor comuníquese con el CAIS.");
                }

                var fwk_domain_name = _LDAPService.Get_correct_DomainName(req.domainName);

                //ActiveDirectoryService.User_Reset_Password(req.userName, req.newPassword, fwk_domain_name);

                DAC.MeucciDAC.ReseteoWeb_Log(req.emp_Id, req.userName, req.dom_id, req.userId, "", Common.changePwdSelft, req.host);
                //DAC.MeucciDAC.ReseteoWeb_EnviosMails(req.emp_Id, req.WindowsUser, req.dom_id, req.ResetUserId, Common.resetear, req.host);
                return apiHelper.fromObject<String>("La contraseña se cambio exitosamente. ");
            }
            catch (Exception ex)
            {

                var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                if (msg.ToLower().Contains("impersonali") || msg.ToLower().Contains("acceso"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.userName + " en el dominio " + req.domainName
                        + " deberá comunicarse  con CAIS ");
                    ex = t;
                }
                if (msg.Contains("E_ACCESSDENIED"))
                {
                    TechnicalException t = new TechnicalException("No es posible resetear " + req.userName + " en el dominio " + req.domainName
                        + " deberá comunicarse con CAIS ");
                    ex = t;
                }
                return apiHelper.fromEx(ex);
            }
        }



        /// <summary>
        /// API que retorna los Empleado con sus usuarios
        /// </summary>
        /// <param name="req">Parametros de entrada</param>
        /// <returns>ok or error</returns>
        [HttpPost]
        [Route("userForgotPassword_checkDNI")]
        [AllowAnonymous]
        public HttpResponseMessage userForgotPassword_checkDNI(forgotPassword_requetsReq req)
        {

            try
            {
               
                var empleado = ActiveDirectoryService.ForgotPassword_checkDNI(req.dni);
                
                return apiHelper.fromObject<EmpleadoBE>(empleado);
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }


        /// <summary>
        /// API que envia un correo al solicitante de cambio de clave
        /// </summary>
        /// <param name="req">Parametros de entrada</param>
        /// <returns>ok or error</returns>
        [HttpPost]
        [Route("forgotPassword_requets")]
        [AllowAnonymous]
        public HttpResponseMessage forgotPassword_requets(forgotPassword_requetsReq req)
        {
            try
            {
             
                var res = ActiveDirectoryService.forgotPassword_requets(req.dni);

          
                return apiHelper.fromObject<ForgotPasswordRes>(res);
            }
            catch (Exception ex)
            {

                //var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, false);
                //if (msg.ToLower().Contains("impersonali") || msg.ToLower().Contains("acceso"))
                //{
                //    TechnicalException t = new TechnicalException("No es posible resetear " + req.dni + " en el dominio " + req.domain
                //        + " deberá comunicarse  con CAIS ");
                //    ex = t;
                //}
                //if (msg.Contains("E_ACCESSDENIED"))
                //{
                //    TechnicalException t = new TechnicalException("No es posible resetear " + req.userName + " en el dominio " + req.domain
                //        + " deberá comunicarse con CAIS ");
                //    ex = t;
                //}
                return apiHelper.fromEx(ex);
            }
        }


        [HttpGet]
        [Route("generateToken")]
        [AllowAnonymous]
        public HttpResponseMessage generateToken(string dni)
        {
            try
            {

                var res = ActiveDirectoryService.forgotPassword_requets(dni);


                return apiHelper.fromObject<ForgotPasswordRes>(res);
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }

        



        /// <summary>
        /// Verifica codigo dni y si es válido retorna un Empleado y sus usuario
        /// Se usa cuano el inicia la pagina EJ: http://localhost:4200/reset/d2a4ad74c340821b16176f2a9d602393
        /// </summary>
        /// <param name="req"></param>
        /// <returns>EmpleadoBE</returns>
        [HttpPost]
        [Route("userForgotPasswordVerify")]
        [AllowAnonymous]
        public HttpResponseMessage userForgotPasswordVerify(userForgotPasswordVerifyReq req)
        {
            try
            {
                var emp = ActiveDirectoryService.forgotPassworChangePassword_Verify( req.code);
                
                return apiHelper.fromObject<EmpleadoBE>(emp);
            }
            catch (Exception ex)
            {

              
                return apiHelper.fromEx(ex);
            }
        }

       







        /// <summary>
        /// Permite hacer un ping al servicio para determinar si esta activo, Método solo para test.
        /// </summary>
        /// <returns>Retorna un mensaje si esta activo</returns>
        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public HttpResponseMessage ping()
        {
            return apiHelper.fromObject<String>("El servicio funciona correctamente");
        }

    }
}