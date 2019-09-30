using chatBotEpironApi.webApi.DAC;
using chatBotEpironApi.webApi.helpers;
using chatBotEpironApi.webApi.models;
using Fwk.chatBotEpironApi.Contracts;
using Fwk.Exceptions;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace chatBotEpironApi.webApi.common
{
    public interface IEpironService
    {
        string Authenticate(string username, string password, string domain);


        void Bot_webhook_update_recivedStatus(SendResponseBotCommentReq req);
        void Bot_update_sendStatus(EnqueueCommentBotReq req);
    }

    public class EpironService : IEpironService
    {
        //private readonly AppSettings _appSettings;
        string currenToken;

        public EpironService()
        {

        }

        void IEpironService.Bot_update_sendStatus(EnqueueCommentBotReq req)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async void Bot_update_sendStatus(EnqueueCommentBotReq req)
        {
            try
            {
                var res = await sendMessaged_async(req);
                if (res.StatusCode == HttpStatusCode.OK)
                {
                    //TODO: desserializar y leer el resultado
                    //Podria venir internamente un error
                    var content = res.Content.ReadAsStringAsync().Result;
                    EpironDAC.Bot_update_sendStatus(req.CaseCommentGUID, "OK", "");
                }
                else
                {
                    var msg = await res.RequestMessage.Content.ReadAsStringAsync();
                    EpironDAC.Bot_update_sendStatus(req.CaseCommentGUID, "Error", msg);
                }

            }
            catch (Exception ex)
            {
                EpironDAC.Bot_update_sendStatus(req.CaseCommentGUID, "Error", ex.Message);
            }

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="req"></param>
        public void Bot_webhook_update_recivedStatus(SendResponseBotCommentReq req)
        {
            //TODO: analizar que pasa si ocurre un error al intentar almacenar la data
            //ver si existe la forma de responder un ACK para q bot lo marque como recibido ok
           
            try
            {
                EpironDAC.Bot_update_response(req.CaseCommentGUID, req.Action,req.Text);
            }
            catch (Exception ex)
            {
                EpironDAC.Bot_update_sendStatus(req.CaseCommentGUID, "Error", ex.Message);
            }

        }
        async Task<HttpResponseMessage> sendMessaged_async(EnqueueCommentBotReq message)
        {

            var url = string.Format("{0}api/send", apiAppSettings.serverSettings.apiConfig.bootApiBaseUrl);


            try
            {
                var httpHandler = apiAppSettings.getProxy_HttpClientHandler();
                using (var httpClient = new HttpClient())

                {
                    string jsonContnt = Fwk.HelperFunctions.SerializationFunctions.SerializeObjectToJson_Newtonsoft(message);
                    StringContent content = new StringContent(jsonContnt, Encoding.UTF8, "application/json");


                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("AuthenticationToken", currenToken);

                    var response = await httpClient.PostAsync(url, content);

                    return response;
                }

            }

            catch (Exception ex)
            {
                return apiAppSettings.getHttpResponseMessage(ex);
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

