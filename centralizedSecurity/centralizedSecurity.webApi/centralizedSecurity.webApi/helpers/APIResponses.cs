using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace CentralizedSecurity.webApi.helpers
{

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