using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using chatBotEpironApi.webApi.common;
using chatBotEpironApi.webApi.helpers;
using chatBotEpironApi.webApi.models;
using chatBotEpironApi.webApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace chatBotEpironApi.webApi.Controllers
{
    [Authorize]
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
        public IActionResult enqueueBotComment(EnqueueCommentBotReq req)
        {
            try
            {
                this.epironService.Bot_update_sendStatus(req);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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

                sql = Common.Get_SqlConnectionStringBuilder(Common.CnnStringNameAD);
                info.SQLServerepiron = "Coneccion :" + Common.CnnStringNameAD + " Sql Server = " + sql.DataSource + " BD " + sql.InitialCatalog;

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

        #endregion













    }
}