using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Threading;
using CentralizedSecurity.webApi.Models;
using Fwk.CentralizedSecurity.helpers;
using Fwk.CentralizedSecurity.Contracts;
using CentralizedSecurity.webApi.helpers;
using System.Web.Http.Cors;
using CentralizedSecurity.webApi.service;

namespace CentralizedSecurity.webApi.Controllers
{
    [JWTAuthentication]
    [RoutePrefix("api/meucci")]
    //[EnableCors(origins: "http://localhost:4200", headers: "*", methods: "*")]
    public class MeucciController : ApiController
    {


        ILDAPService _LDAPService;

        public MeucciController()
        {

            _LDAPService = LDAPService.CreateInstance();
        }

        //Verifica si existe un usuario en un dominio
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
                        throw new Fwk.Exceptions.FunctionalException("El DNI ingresado no existe o no tiene visibilidad sobre el mismo.Por favor comuníquese con el CAIS.");
                    }
                }


                //empleado <> NULL
                // Si Aus_Id un valor distinto de NULL, 
                if (empleado.Aus_Id.HasValue)
                {
                    //2.Si la ejecución del SP devuelve en el campo Aus_Id un valor distinto de NULL, sin importar si el usuario pertenece o no al CAIS se deberá mostrar el mensaje 
                    throw new Fwk.Exceptions.FunctionalException("No es posible resetear o desbloquear " +
                        "el Usuario Windows porque el empleado se encuentra ausente. Ante la duda, " +
                        "comuníquese con el ACI”, no permitiendo resetear o desbloquear dicho UW. ");
                }



                var resp = apiHelper.fromObject<EmpleadoBE>(empleado);
                return resp;
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("retriveDomains")]
        public HttpResponseMessage retriveDomains(LoginRequest login)//string userName, string domain, string dni)
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


        //Metodo solo para test.
        [AllowAnonymous]
        [HttpGet]
        [Route("ping")]
        public HttpResponseMessage ping()
        {
            return apiHelper.fromObject<String>("El servicio funciona correctamente");
        }



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