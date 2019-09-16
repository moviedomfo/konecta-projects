using CentralizedSecurity.webApi.DAC;
using CentralizedSecurity.webApi.helpers;
using Fwk.CentralizedSecurity.Contracts;
using Fwk.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace CentralizedSecurity.webApi.common
{
    public interface IMeucciService
    {
        string Authenticate(string username, string password, string domain);
        
    }

    public class MeucciService : IMeucciService
    {
        //private readonly AppSettings _appSettings;


        public MeucciService()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        public String Authenticate(string username, string password, string domain)
        {

            FunctionalException fe = null;
            try
            {
                var domName = ActiveDirectoryService.Get_correct_DomainName(domain);
                ///Virifica contra domino
                var res = ActiveDirectoryService.User_Logon(username, password, domName);
                //var resp = apiHelper.fromObject<LoogonUserResult>(res);

                if (res.LogResult == "LOGIN_USER_OR_PASSWORD_INCORRECT")
                //    if (res.Autenticated == false)
                {
                    fe = new FunctionalException("El usuario y/o contraseña es incorrecto");
                    fe.ErrorId = HttpStatusCode.Unauthorized.ToString();
                    throw fe;
                    //return BadRequest(new ApiErrorResponse(HttpStatusCode.Unauthorized, "El usuario y/o contraseña es incorrecto"));
                }

                if (res.LogResult == "LOGIN_USER_DOESNT_EXIST")
                {
                    fe = new FunctionalException("El usuario no existe en el dominio ");
                    fe.ErrorId = HttpStatusCode.Unauthorized.ToString();
                    throw fe;
                    //return BadRequest(new ApiErrorResponse(HttpStatusCode.Unauthorized, "El usuario no existe en el dominio "));
                }

                //si la verificacion contra dominio es OK
                //busco info del dmonio 
                int dom_id = MeucciDAC.GetDimainId(domain);




                var emmpleadoBE = MeucciDAC.VirifyUser(username, dom_id);

                //Emp_Id, legajo correspondiente al usuario reseteador, si devuelve NULL mostrar el mensaje “Usuario no registrado en Meucci” y cerrar aplicación.
                //o Cue_Id, cuenta correspondiente al usuario reseteador, si devuelve NULL y el campo CAIS es 0, mostrar el mensaje “Usuario no habilitado” 

                if (emmpleadoBE == null)
                {
                    fe = new FunctionalException("Usuario no registrado en Meucci ");
                    fe.ErrorId = HttpStatusCode.Unauthorized.ToString();
                    throw fe;
                    //return BadRequest(new ApiErrorResponse(HttpStatusCode.Unauthorized, "Usuario no registrado en Meucci "));
                }

                if (string.IsNullOrEmpty(emmpleadoBE.Cuenta) && emmpleadoBE.CAIS == false)
                {
                    fe = new FunctionalException("Usuario no habilitado ");
                    fe.ErrorId = HttpStatusCode.Unauthorized.ToString();
                    throw fe;
                    //return BadRequest(new ApiErrorResponse(HttpStatusCode.Unauthorized, "Usuario no habilitado"));
                }
                emmpleadoBE.Dominio = domain;


                var jwtTokenString = TokenGenerator.GenerateTokenMeucci(emmpleadoBE);
                return jwtTokenString;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
       

}

