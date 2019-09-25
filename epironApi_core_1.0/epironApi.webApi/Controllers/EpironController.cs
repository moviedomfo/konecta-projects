using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using epironApi.webApi.common;
using epironApi.webApi.helpers;
using epironApi.webApi.models;
using epironApi.webApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace epironApi.webApi.Controllers
{
    ///[Authorize]
    [Route("/api/epiron")]
    [ApiController]
    public class EpironController : ControllerBase
    {
        private readonly IEpironService epironService;
  
        public EpironController(IEpironService epironService)
        {
            this.epironService = epironService;
            
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




        [HttpPost("[action]")]
        public IActionResult enqueueBotComment(EnqueueCommentBotReq req)
        {
            return Ok("El ping secure funciona correctamente");
        }



        /// <summary>
        /// web hook de bot api
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult eendResponseBotCommentReq(SendResponseBotCommentReq req)
        {
            return Ok("El ping secure funciona correctamente");
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


    }
}