using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using EpironAPI.BE;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using EpironAPI.Models;

namespace EpironAPI
{
    public class apiHelper
    {
        static bool dispatcherErrorHandlerGetStackTrace = false;
        static apiHelper()
        {

            if (System.Configuration.ConfigurationManager.AppSettings["dispatcherErrorHandlerGetStackTrace"] != null)
            {
                bool.TryParse(System.Configuration.ConfigurationManager.AppSettings["dispatcherErrorHandlerGetStackTrace"].ToString(), out dispatcherErrorHandlerGetStackTrace);
            }

        }



        public static HttpResponseMessage fromEx(Exception ex, HttpStatusCode code = HttpStatusCode.InternalServerError)
        {
           

            if (ex.InnerException != null)
            {
                if (ex.InnerException.InnerException != null)
                    return get_HttpResponseMessage(ex.InnerException.InnerException);
                else
                    return get_HttpResponseMessage(ex.InnerException);
            }
            return get_HttpResponseMessage(ex);
           
        }
        static HttpResponseMessage get_HttpResponseMessage(Exception ex)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(ex.Message),
                ReasonPhrase = "Error"
            };

            return resp;
        }


        public static HttpResponseMessage fromErrorString(string message, HttpStatusCode status)
        {
            EpironApiResponse res = new EpironApiResponse(null);
            res.Error = new Error();
            res.Error.EventResponseText = message;
            res.Error.EventResponseInternalCode = -2000;
            var resp = new HttpResponseMessage(status)
            {
                Content = new StringContent(message),
            };

            return resp;
        }


        /// <summary>
        /// retorna un HttpResponseMessage con un objeto EpironApiResponse
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <param name="errorResponse"></param>
        /// <returns></returns>
        public static HttpResponseMessage fromObject<T>(T obj, Error errorResponse=null, HttpStatusCode statusCode= HttpStatusCode.OK)
        {
            

            EpironApiResponse res = new EpironApiResponse(obj);
            res.Error = errorResponse;
            res.StatusCode = statusCode;
         
            var resp = new HttpResponseMessage(res.StatusCode)
            {
                Content = new ObjectContent(typeof(EpironApiResponse), res, GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
            return resp;
        }
      
        /// <summary>
        /// 
        /// </summary>
        /// <param name="errorResponse"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        public static HttpResponseMessage fromError(Error errorResponse = null, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {


            EpironApiResponse res = new EpironApiResponse(null);
            res.Error = errorResponse;
            res.StatusCode = statusCode;

            var resp = new HttpResponseMessage(res.StatusCode)
            {
                Content = new ObjectContent(typeof(EpironApiResponse), res, GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
            return resp;
        }

        public static HttpResponseMessage fromObject(Object obj, Error errorResponse = null, HttpStatusCode statusCode = HttpStatusCode.OK)
        {


            EpironApiResponse res = new EpironApiResponse(obj);
            res.Error = errorResponse;
            res.StatusCode = statusCode;
         

            var resp = new HttpResponseMessage(res.StatusCode)
            {
                Content = new ObjectContent(typeof(EpironApiResponse), res, GlobalConfiguration.Configuration.Formatters.JsonFormatter)


            };
            return resp;
        }

    }



    public class ApiOkResponse : ApiResponse
    {
        public object ServiceData { get; }
        public Error Errors { get; set; }
        public ApiOkResponse(object result) : base((int)HttpStatusCode.OK)
        {
            ServiceData = result;
        }

        public ApiOkResponse(object result, HttpStatusCode statusCode) : base((int)statusCode)
        {
            ServiceData = result;
        }
    }
    public class ApiErrorResponse : ApiResponse
    {
        public ApiErrorResponse(HttpStatusCode statusCode, string message = null) : base((int)statusCode, message)
        {
        }
        //public ApiErrorResponse(int statusCode, Exception ex) : base(statusCode)
        //{



        //}

        public IEnumerable<string> Errors { get; }


    }
    public class ApiResponse
    {
        public int StatusCode { get; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Message { get; }

        public ApiResponse(int statusCode, string message = null)
        {
            StatusCode = statusCode;
            Message = message ?? GetDefaultMessageForStatusCode(statusCode);
        }

        private static string GetDefaultMessageForStatusCode(int statusCode)
        {
            switch (statusCode)
            {

                case 404:
                    return "Resource not found";
                case 500:
                    return "An unhandled error occurred";
                default:
                    return null;
            }
        }



    }


    public class EpironApiResponse
    {
        public EpironApiResponse(object result)
        {
            Result = result;
        }

        
        public HttpStatusCode StatusCode { get; set; }
        public Error Error { get; set; }
        public  Object Result { get; set; }
    }
}
