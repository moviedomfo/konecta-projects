using Fwk.Exceptions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web;
using System.Web.Http;

namespace CentralizedSecurity.webApi
{
    internal class apiHelper
    {
        public static apiConfig apiConfig = null;
        public static void InitializeConfig(string path)
        {
            try
            {
                 apiConfig = apiConfig.CreateNew(path);

                //if (!System.IO.Directory.Exists(apiConfig.logsFolder))
                //    System.IO.Directory.CreateDirectory(apiConfig.logsFolder);
                //setProxy();
            }
            catch (Exception ex)
            {
                //Log_FileSystem(ex);
                throw ex;
            }
        }

        public static String GetAllMessageException(Exception ex, bool includeStackTrace = true)
        {
            StringBuilder wMessage = new StringBuilder();
            
            wMessage.Append(ex.Message);
            wMessage.AppendLine();
            while (ex.InnerException != null)
            {
                ex = ex.InnerException;
                wMessage.AppendLine("\r\n-----------Error interno------------------\r\n");
                if(!string.IsNullOrEmpty(ex.Source))
                    wMessage.AppendLine(String.Concat("Origen: ", ex.Source));

                wMessage.AppendLine("Error: ");
                wMessage.AppendLine(ex.Message);
            }
            if (includeStackTrace && !String.IsNullOrEmpty(ex.StackTrace))
            {
                wMessage.AppendLine("\r\n-----------StackTrace------------------\r\n");
                wMessage.AppendLine(ex.StackTrace);
            }
            return wMessage.ToString();
        }

        internal static HttpResponseMessage fromEx(Exception ex)
        {
            Exception currentErr = null;
            string errorId = "Error";

            if (ex.GetType() == typeof(Fwk.Exceptions.TechnicalException))
            {
                currentErr = ex;
            }
            if (ex.GetType() == typeof(Fwk.Exceptions.FunctionalException))
            {
                errorId = ((FunctionalException)ex).ErrorId;
                currentErr = ex;
            }
            if (currentErr == null && ex.InnerException != null)
                currentErr = ex.InnerException;

            if (currentErr == null)
                currentErr = ex;


            

            var msg = GetAllMessageException(currentErr, false);
            var resp = new HttpResponseMessage(HttpStatusCode.InternalServerError)
            {
                
                Content = new StringContent(msg),
                ReasonPhrase = errorId
            };

            return resp;
        }
        internal static HttpResponseException fromEx2(Exception ex, HttpStatusCode statusCode= HttpStatusCode.InternalServerError)
        {
            Exception currentErr = null;

            string exType = "TechnicalException";
            if (ex.GetType() == typeof(Fwk.Exceptions.TechnicalException))
            {
                currentErr = ex;
                exType = "FunctionalException";
            }
            if (ex.GetType() == typeof(Fwk.Exceptions.FunctionalException))
            {
                currentErr = ex;
                exType = "FunctionalException";
            }

            if (currentErr == null && ex.InnerException != null)
                currentErr = ex.InnerException;

            if (currentErr == null)
                currentErr = ex;



            var msg = GetAllMessageException(currentErr, false);
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(msg),
                
                //ReasonPhrase = exType
            };
                    
            return new HttpResponseException(response);
        }

        internal static HttpResponseException fromErrorString2(string message, HttpStatusCode statusCode = HttpStatusCode.InternalServerError)
        {
            FunctionalException ex = new FunctionalException(message);


            return fromEx2( ex,  statusCode);
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
}