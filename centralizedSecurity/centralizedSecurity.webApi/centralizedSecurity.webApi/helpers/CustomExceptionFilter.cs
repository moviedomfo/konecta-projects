using System;
using System.Net;  
    using System.Net.Http;  
    using System.Web.Http.Filters;  
  
namespace CentralizedSecurity.webApi
{
 
    public class CustomExceptionFilter : ExceptionFilterAttribute
    {
        public override void OnException(HttpActionExecutedContext actionExecutedContext)
        {
            string exceptionMessage = string.Empty;
            Exception currentErr = actionExecutedContext.Exception;
            if (actionExecutedContext.Exception.InnerException == null)
            {
                exceptionMessage = actionExecutedContext.Exception.Message;
            }
            else
            {
                exceptionMessage = actionExecutedContext.Exception.InnerException.Message;
            }

            exceptionMessage = apiHelper.GetAllMessageException(currentErr, false);

            //We can log this exception message to the file or database.  
            var response = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                Content = new StringContent(exceptionMessage),  
                ReasonPhrase = "Internal Server Error. Please Contact your Administrator."
            };
            actionExecutedContext.Response = response;
        }
    }
}