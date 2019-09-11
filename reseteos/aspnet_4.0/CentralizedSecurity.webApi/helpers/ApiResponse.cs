using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace CentralizedSecurity.webApi
{

    internal class apiHelper
    {
        internal static HttpResponseMessage fromEx(Exception ex)
        {
            Exception currentErr=null;

            if (ex.GetType() == typeof(Fwk.Exceptions.TechnicalException))
            {
                currentErr = ex;
            }

            if (currentErr ==null && ex.InnerException != null)
                currentErr = ex.InnerException;

            if (currentErr == null)
                currentErr = ex;



            var msg = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(currentErr, false);
            var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(msg),
                ReasonPhrase = "Error"
            };

            return resp;
        }
        internal static HttpResponseMessage fromErrorString(string message, HttpStatusCode status)
        {


            var resp = new HttpResponseMessage(status)
            {
                Content = new StringContent(message),
            };

            return resp;
        }


        internal static HttpResponseMessage fromObject<T>(T obj)
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