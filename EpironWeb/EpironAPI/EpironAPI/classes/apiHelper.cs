using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

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

        public static HttpResponseMessage fromObject<T>(T obj)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ObjectContent(typeof(T), obj, GlobalConfiguration.Configuration.Formatters.JsonFormatter)
            };
            return resp;
        }

    }



    public class ApiOkResponse : ApiResponse
    {
        public object Result { get; }

        public ApiOkResponse(object result)
            : base(200)
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
}
