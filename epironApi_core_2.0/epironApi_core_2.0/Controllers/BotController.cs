using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using epironApi.webApi.common;
using epironApi.webApi.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace epironApi.webApi.Controllers
{
    ///[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {
        private readonly IEpironService epironService;
  
        /// <summary>
        /// 
        /// </summary>
        /// <param name="epironService"></param>
        public BotController(IEpironService epironService)
        {
            this.epironService = epironService;
            
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
        /// web hook de bot api
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult resesponseBotComment(SendResponseBotCommentReq req)
        {
            //TODO: analizar si existe un api que me retorne comment enviados por sin respuesta
            try
            {
                epironService.Bot_webhook_update_recivedStatus(req);
             
                return Ok();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
            
            
        }

        




    }
}