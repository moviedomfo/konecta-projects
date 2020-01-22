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
            //if (ex.InnerException.InnerException != null)
            //    ex = ex.InnerException.InnerException;

            //if (ex == null && ex.InnerException != null)
            //    ex = ex.InnerException;


            var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex, dispatcherErrorHandlerGetStackTrace);
            var resp = new HttpResponseMessage(code)
            {
                Content = new StringContent(msg),
                ReasonPhrase = "Error"
            };

            return resp;
        }
        public static HttpResponseMessage fromErrorString(string message, HttpStatusCode status)
        {


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
            res.Errors = errorResponse;
            res.StatusCode = statusCode;
            //if (typeof(EpironApiResponse).IsAssignableFrom(typeof(T)))
            //{
            //    var o = obj as EpironApiResponse;
            //    status = o.StatusCode;


            //}

            var resp = new HttpResponseMessage(res.StatusCode)
            {
                Content = new ObjectContent(typeof(EpironApiResponse), res, GlobalConfiguration.Configuration.Formatters.JsonFormatter)


            };
            return resp;
        }

    }



    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }
        public Error Errors { get; set; }
        public ApiOkResponse(object result) : base((int)HttpStatusCode.OK)
        {
            Result = result;
        }

        public ApiOkResponse(object result, HttpStatusCode statusCode) : base((int)statusCode)
        {
            Result = result;
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
        public Error Errors { get; set; }
        public  Object Result { get; set; }
    }
}
