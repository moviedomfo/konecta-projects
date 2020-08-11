using epironApi.webApi.DAC;
using epironApi.webApi.helpers;
using epironApi.webApi.models;
using Fwk.epironApi.Contracts;
using Fwk.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using EpironApi.webApi.BC;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using System.Data.SqlClient;

namespace epironApi.webApi.common
{
  
    public interface IEpironService
    {
        string Authenticate(string username, string password, string domain);

        Task Bot_update_sendStatus(EnqueueCommentBotReq req);

        void Bot_webhook_update_recivedStatus(BotCommentModeratedReq req);
    }
    public class EpironService : IEpironService
    {
        //private readonly AppSettings _appSettings;
        string currenToken;

        public EpironService()
        {

        }



        /// <summary>
        /// 1 - actualiza la BD BotCreatedDateObtained
        /// 2 - Envia al moderardor
        /// 3 - update_delivery
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task Bot_update_sendStatus(EnqueueCommentBotReq req)
        {
            string msgError = "";
            try
            {
                int count = EpironDAC.MessageBot_u_BotCreatedDateObtained(req);
                if (count == 0)
                {
                    msgError = " No se encontro CaseCommentGUID  " + req.CaseCommentGUID;
                    throw new Exception(msgError);
                }
                
                var res = await sendMessaged_async(req);

                if (res.StatusCode == HttpStatusCode.OK)
                {
                    //TODO: desserializar y leer el resultado
                    //Podria venir internamente un error
                    var content = res.Content.ReadAsStringAsync().Result;
                    //EpironDAC.Bot_update_delivery(req.CaseCommentGUID,req.CaseId);
                }
                else
                {
                    if(res.StatusCode== HttpStatusCode.Unauthorized)
                    {
                        msgError = "No tiene autorización para acceder a " + apiAppSettings.serverSettings.apiConfig.api_bootApiHoock;
                    }
                    else
                    {
                        msgError = res.ReasonPhrase;
                    }
                    //var msg = await res.RequestMessage.Content.ReadAsStringAsync();
                    
                    //EpironDAC.MessageBot_u_BotCreatedDateObtained(req);
                    //EpironDAC.Bot_update_sendStatus(req.CaseCommentGUID, "Error", msg);
                }

            }
            catch (Exception ex)
            {
                msgError = ex.Message;

             //   EpironDAC.Bot_update_sendStatus(req.CaseCommentGUID, "Error", ex.Message);
            }

            if (!string.IsNullOrEmpty(msgError))
            {
                throw new Exception(msgError);
            }
        }

        /// <summary>
        /// Se actualiza el registro con la respuesta enviada desde la APi Bot
        /// </summary>
        /// <param name="req"></param>
        public void Bot_webhook_update_recivedStatus(BotCommentModeratedReq req)
        {
            //TODO: analizar que pasa si ocurre un error al intentar almacenar la data
            //ver si existe la forma de responder un ACK para q bot lo marque como recibido ok

            try
            {
               
                  
                    Update_recivedStatusBC bc = new Update_recivedStatusBC();
                    bc.Proc(req);

                 
            }
            catch (Exception ex)
            {
                throw ex;
                //EpironDAC.Bot_update_sendStatus(req.CaseCommentGUID, "Error", ex.Message);
            }

        }

        async Task<HttpResponseMessage> sendMessaged_async(EnqueueCommentBotReq message)
        {
            AppContext.SetSwitch("System.Net.Http.UseSocketsHttpHandler", false);
            
            // var url = string.Format("{0}/api/bot/moderateBotComment", apiAppSettings.serverSettings.apiConfig.api_bootApiHoock);
            var url = apiAppSettings.serverSettings.apiConfig.api_bootApiHoock;
            if(string.IsNullOrEmpty(url))
                throw new Exception("Falta configurar  api_bootApiHoock ");

            Uri uri = new Uri(url);
            try
            {
                var httpHandler = apiAppSettings.getProxy_HttpClientHandler();
              //  using (var httpClient = new HttpClient(httpHandler))
                using (var httpClient = new HttpClient())
                {
                    
                    string jsonContnt = Fwk.HelperFunctions.SerializationFunctions.SerializeObjectToJson_Newtonsoft(message);
                    
                    HttpContent content = new StringContent(jsonContnt, Encoding.UTF8, "application/json");
                    HttpRequestMessage request = new HttpRequestMessage
                    {
                        Method = HttpMethod.Post,
                        RequestUri = uri,
                        Content = content
                    };

                    //httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    //httpClient.DefaultRequestHeaders.TryAddWithoutValidation("AuthenticationToken", currenToken);

                    HttpResponseMessage result = await httpClient.PostAsync(uri, content);
                 //   if (response.IsSuccessStatusCode)
                 //   {
                 //       var readTask = result.Content.ReadAsAsync<String>();
                 //       readTask.Wait();
                 //       res = readTask.Result;

                 //   }
                 //   else
                 //   {
                 //       res = result.StatusCode.ToString();

                 //       Fwk.Exceptions.TechnicalException te =
                 //new Fwk.Exceptions.TechnicalException("Error al intentar enviar un msg a " + apiAppSettings.apiConfig.api_epironHoockApi + " StatusCode : " + result.StatusCode.ToString());



                 //   }
                    return result;
                }

            }

            catch (Exception ex)
            {
                Exception e = new Exception("Error al enviar el mensaje al moderador " ,ex);

                return apiAppSettings.getHttpResponseMessage(e);
            }

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

                var emmpleadoBE = EpironDAC.VirifyUser(username);

                //Emp_Id, legajo correspondiente al usuario reseteador, si devuelve NULL mostrar el mensaje “Usuario no registrado en Epiron” y cerrar aplicación.
                //o Cue_Id, cuenta correspondiente al usuario reseteador, si devuelve NULL y el campo CAIS es 0, mostrar el mensaje “Usuario no habilitado” 

                if (emmpleadoBE == null)
                {
                    fe = new FunctionalException("Usuario no registrado en Epiron ");
                    fe.ErrorId = HttpStatusCode.Unauthorized.ToString();
                    throw fe;
                    //return BadRequest(new ApiErrorResponse(HttpStatusCode.Unauthorized, "Usuario no registrado en Epiron "));
                }

                if (string.IsNullOrEmpty(emmpleadoBE.Cuenta) && emmpleadoBE.CAIS == false)
                {
                    fe = new FunctionalException("Usuario no habilitado ");
                    fe.ErrorId = HttpStatusCode.Unauthorized.ToString();
                    throw fe;
                    //return BadRequest(new ApiErrorResponse(HttpStatusCode.Unauthorized, "Usuario no habilitado"));
                }
                emmpleadoBE.Dominio = domain;


                var jwtTokenString = TokenGenerator.GenerateTokenEpiron(emmpleadoBE);
                return jwtTokenString;

            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }


}

