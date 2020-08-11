using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using epironApi.webApi.common;
using epironApi.webApi.helpers;
using epironApi.webApi.models;
using epironApi.webApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace epironApi.webApi.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EpironController : ControllerBase
    {
        private readonly IEpironService epironService;

        public EpironController(IEpironService epironService)
        {
            this.epironService = epironService;

        }
        /// <summary>
        /// Este metodo es utilizado por Epiron para informar de algun comentario o mensaje de usuario :
        /// El mensaje sera enviado a la api de bot. La api contestara inmediatemaente reibido Ok  o un error
        /// Pero el resultado del procesamiento del mensaje sera lelvado a cabo de forma asincrona y sera recibiodo por un hook
        ///api/Bot/resesponseBotComment
        ///
        /// La api enqueueBotComment.. almacenara en la BD el resulatado y estado actual del mensaje
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public async Task<IActionResult> enqueueBotComment(EnqueueCommentBotReq req)
        {
            try
            {

                await this.epironService.Bot_update_sendStatus(req);

                return Ok(new ApiOkResponse("Ok enviado CaseId = " + req.CaseId + " url " + apiAppSettings.serverSettings.apiConfig.api_bootApiHoock));
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, ex.Message));
                
            }
        }



        #region helpers
        /// <summary>
        /// Metodo solo para test contra un jwt.
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult pingSecure()
        {
            return Ok(new ApiOkResponse("El ping secure funciona correctamente"));
            
        }

        /// <summary>
        /// Metodo solo para test.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("[action]")]
        public IActionResult ping()
        {
            return Ok(new ApiOkResponse(string.Format( "El servicio Api Epiron host= {0}  e IP = {0}  funciona correctamente",
                Dns.GetHostName(), 
                Common.Get_IPAddress())));
            
        }
        /// <summary>
        /// Metodo solo para test.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public IActionResult pingPost()
        {
            return Ok(new ApiOkResponse(string.Format("El servicio Api Epiron host= {0}  e IP = {0}  funciona correctamente",
                Dns.GetHostName(),
                Common.Get_IPAddress())));

        }
        /// <summary>
        /// Retorna informacion del servidor de konecta bot web-api
        /// </summary>
        /// <returns></returns>
        [HttpGet("[action]")]
        public IActionResult GetServerInfo()
        {
            try
            {

                ApiServerInfo info = new ApiServerInfo();
                var sql = Common.Get_SqlConnectionStringBuilder(Common.CnnStringNameepiron);
                info.SQLServerepiron = "Coneccion :" + Common.CnnStringNameepiron + " Sql Server = " + sql.DataSource + " BD " + sql.InitialCatalog;

                //sql = Common.Get_SqlConnectionStringBuilder(Common.CnnStringNameAD);
                //info.SQLServerepiron = "Coneccion :" + Common.CnnStringNameAD + " Sql Server = " + sql.DataSource + " BD " + sql.InitialCatalog;
                info.api_bootApiHoock = apiAppSettings.serverSettings.apiConfig.api_bootApiHoock;
                info.Ip = Common.Get_IPAddress();
                info.HostName = Dns.GetHostName();
                
                return Ok(new ApiOkResponse(info));

            }
            catch (Exception ex)
            {
                var msg = apiHelper.getMessageException(ex);
                return BadRequest(new ApiErrorResponse(HttpStatusCode.InternalServerError, msg)); ;
            }
        }

        #endregion


    }
}