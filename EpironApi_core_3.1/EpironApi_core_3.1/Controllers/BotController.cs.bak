using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using epironApi.webApi.common;
using epironApi.webApi.helpers;
using epironApi.webApi.models;
using epironApi.webApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace epironApi.webApi.Controllers
{
    [Authorize]
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
        /// Metodo solo para test.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost("[action]")]
        public async Task<IActionResult> authenticate(LoginRequest login)
        {
            var url = apiAppSettings.serverSettings.apiConfig.api_authServerBaseUrl + "/api/epiron/authenticate";
            Uri uri = new Uri(url);
            var logingJson = Fwk.HelperFunctions.SerializationFunctions.SerializeObjectToJson_Newtonsoft(login);
            HttpContent c = new StringContent(logingJson, Encoding.UTF8, "application/json");

            var proxy = apiHelper.getProxy_HttpClientHandler();

            using (var client = new HttpClient(proxy))
            {
                HttpRequestMessage request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = uri,
                    Content = c
                };
                HttpResponseMessage result = await client.SendAsync(request);

                //HttpResponseMessage result = await client.PostAsync(url, c);

                if (result.IsSuccessStatusCode)
                {
                    var jwt = await result.Content.ReadAsAsync<String>();
                    return Ok(jwt);
                }
                else
                {
                    
                    return BadRequest(result.Content.ToString());
                }
            }

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