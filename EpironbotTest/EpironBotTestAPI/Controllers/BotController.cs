using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace EpironBotTestAPI.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class BotController : ControllerBase
    {


        public BotController()
        {



        }

        // GET api/values
        [HttpGet("[action]")]
        public IActionResult  check()
        {
            return  Ok("API bot funciona ok");
        }

        [HttpPost("[action]")]
        public IActionResult moderateBotComment(EnqueueCommentBotReq comment)
        {
            try
            {
                // almacenar el msg para ser moderado en la app frontend de test
                comment.ArrivedDate = System.DateTime.Now;
                QueueEnjine.encolar_stored(comment);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        ///  Respuesta del API Bot 
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        [HttpPost("[action]")]
        public IActionResult responseModeratedBotComment(BotCommentModeratedReq comment)
        {
            try
            {
                // enviar msg moderado a ChtBot API resesponseBotComment 
                var item = QueueEnjine.get_stored(comment.CaseCommentGUID);
                if (item != null)
                {
                    item.SendedDate = System.DateTime.Now;
                    QueueEnjine.encolar_sended(item);

                    QueueEnjine.Remove_stored(comment.CaseCommentGUID);
                    
                }


                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("[action]")]
        public IActionResult retriveAllComments()
        {
            try
            {
                // enviar msg moderado a ChtBot API resesponseBotComment 
                return Ok(QueueEnjine.getAll_stored());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


    }
}