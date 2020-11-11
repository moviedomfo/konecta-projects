using EpironAPI.BE;
using EpironAPI.classes;
using EpironAPI.Model;
using EpironAPI.Models;
using Fwk.HelperFunctions;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Security;

namespace EpironAPI.Controllers
{
    [RoutePrefix("api/disp")]
    public class DispController : BaseApiController
    {

        WhiteListSVC whiteListSVC;

        public DispController()
        {
            generateWhiteList();
            DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
            //create a command of the proper type.
            DbConnection conn = factory.CreateConnection();




        }
        /// <summary>
        /// Genera lista de servicios que no requieren autorización
        /// </summary>
        void generateWhiteList()
        {
            if(whiteListSVC == null || whiteListSVC.whiteList .Count==0)
            {
                try {
                   // DatabaseFactory.SetDatabaseProviderFactory(new DatabaseProviderFactory());
                }
                catch { } 
                
            }
            if (System.IO.File.Exists("whiteListSVC.json"))
            {
                string str = System.Web.HttpContext.Current.Server.MapPath("~");
                string wList = FileFunctions.OpenTextFile("whiteListSVC.json");
                whiteListSVC = Fwk.HelperFunctions.SerializationFunctions.DeSerializeObjectFromJson_Newtonsoft(typeof(WhiteListSVC), wList) as WhiteListSVC;
            }
            else
            {
                whiteListSVC = new WhiteListSVC();
                whiteListSVC.whiteList = new List<WhiteListItem>();
                whiteListSVC.whiteList.Add(new WhiteListItem { serviceName = "AuthenticateUserService" });
                whiteListSVC.whiteList.Add(new WhiteListItem { serviceName = "SearchParametroByParamsService" });
                whiteListSVC.whiteList.Add(new WhiteListItem { serviceName = "GetHealthInstitutionByIdService" });
                whiteListSVC.whiteList.Add(new WhiteListItem { serviceName = "RetriveHealthInstitutionService" });
            }
        }

        
        [Route("execute")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Execute(ExecuteReq req)
        {
            try
            {
                DbProviderFactory factory = DbProviderFactories.GetFactory("System.Data.SqlClient");
                //create a command of the proper type.
                DbConnection conn = factory.CreateConnection();
                #region virify required
                if (string.IsNullOrEmpty(req.serviceProviderName))
                    return apiHelper.fromErrorString("serviceProviderName es requerido", HttpStatusCode.NoContent);
                if (string.IsNullOrEmpty(req.serviceName))
                    return apiHelper.fromErrorString("serviceName es requerido", HttpStatusCode.NoContent);
                if (string.IsNullOrEmpty(req.jsonRequest.ToString()))
                    return apiHelper.fromErrorString("jsonRequest es requerido", HttpStatusCode.NoContent);

                #endregion


                base.CreateSimpleFacade();

                //var resToJson = base.simpleFacade.ExecuteServiceJson_newtonjs(req.serviceProviderName, req.serviceName, req.jsonRequest.ToString(), base.hostContext);
                var res = base.simpleFacade.ExecuteServiceJsonBase(req.serviceProviderName, req.serviceName, req.jsonRequest.ToString(), base.hostContext);
                return apiHelper.fromObject<ApiOkResponse>(new ApiOkResponse(res));

            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }


        [AllowAnonymous]
        [Route("executeWhiteList")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage ExecuteWhiteList(ExecuteReq req)
        {
            try
            {

                #region virify 
                //if (string.IsNullOrEmpty(req.serviceProviderName))
                //    return apiHelper.fromErrorString("serviceProviderName es requerido", HttpStatusCode.NoContent);
                if (string.IsNullOrEmpty(req.serviceName))
                    return apiHelper.fromErrorString("serviceName es requerido", HttpStatusCode.NoContent);
                if (string.IsNullOrEmpty(req.jsonRequest.ToString()))
                    return apiHelper.fromErrorString("jsonRequest es requerido", HttpStatusCode.NoContent);

                if (whiteListSVC.whiteList.Any(s => s.serviceName.Equals(req.serviceName)) == false)
                {
                    HttpResponseMessage wHttpResponseMessage = apiHelper.fromErrorString(String.Format("Error the service {0} was not allowed",
                        req.serviceName),
                        HttpStatusCode.Unauthorized);
                    return wHttpResponseMessage;
                }
                #endregion


                base.CreateSimpleFacade();




                var resToJson = base.simpleFacade.ExecuteServiceJson_newtonjs(req.serviceProviderName, req.serviceName, req.jsonRequest.ToString(), base.hostContext);

                return apiHelper.fromObject<ApiOkResponse>(new ApiOkResponse(resToJson));
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }


        [HttpGet]
        [AllowAnonymous]
        [Route("check")]
        public HttpResponseMessage check()
        {

            //var res = new { message = "La API funciona correctamente" };

            return apiHelper.fromErrorString("La API funciona correctamente", HttpStatusCode.OK);
        }

       

     

       

    }
}
