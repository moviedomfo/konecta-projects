
using Fwk.DataBase;
using Fwk.Exceptions;
using Fwk.HelperFunctions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace epironApi.webApi.helpers
{
    public class apiHelper
    {

        public static string getMessageException(Exception ex)
        {
            string msg = string.Empty;
            //if (ex.GetType() == typeof(HttpResponseException))
            //        msg = ex.Message;
            //return msg = ex.Message;

            if(ex.GetType() == typeof(TechnicalException))
            {
                var te = ex as TechnicalException;

                msg = te.Message;
                //return msg;
            }
            if (ex.InnerException != null)
            {

                msg = msg + ex.InnerException.Message;
                if (ex.InnerException.GetType() == typeof(System.Net.Sockets.SocketException))
                {
                    var e = ex.InnerException as System.Net.Sockets.SocketException;
                    if (e.ErrorCode == 10060)
                        msg =   "La url no es accesible " + Environment.NewLine + msg;
                }
                //if (ex.InnerException.GetType() == typeof(WebExcepcion))
                //{
                //    var e = ex.InnerException as System.Net.Sockets.SocketException;
                //    if (e.ErrorCode == 10060)
                //        msg = "WAPI wapiAppSettings.apiConfig.apiDomain no es accesible " + Environment.NewLine + msg;
                //}
            }

            else
                msg = ex.Message;

            return msg;
        }


        public static HttpClientHandler getProxy_HttpClientHandler()
        {
            HttpClientHandler httpClientHandler = null;
            if (apiAppSettings.serverSettings.apiConfig.proxyEnabled)
            {
                var proxy = new WebProxy()
                {
                    Address = new Uri(string.Format("http://{0}:{1}", apiAppSettings.serverSettings.apiConfig.proxyName, apiAppSettings.serverSettings.apiConfig.proxyPort)),
                    //BypassOnLocal = false,
                    UseDefaultCredentials = true

                    // *** These creds are given to the proxy server, not the web server ***
                    //Credentials = new NetworkCredential(
                    //    userName: wapiAppSettings.apiConfig.proxyUser,
                    //    password: wapiAppSettings.apiConfig.proxyPassword)
                };


                httpClientHandler = new HttpClientHandler()
                {
                    Proxy = proxy,
                    UseProxy = true,
                    PreAuthenticate = false,
                    UseDefaultCredentials = true

                };

            }

            return httpClientHandler;
        }

      

       

        #region Log_NonDatabase


        /// <summary>
        /// Intenta almacenar eventos en file system
        /// </summary>
        /// <param name="ev"></param>
        //public static void Log_FileSystem(Event ev)
        //{
        //    if (ev == null) return;

        //    try
        //    {

        //        string prefix_File = Fwk.HelperFunctions.FormatFunctions.ToYYYYMMDD(System.DateTime.Now, '-');

        //        string fullFileName = System.IO.Path.Combine(apiConfig.logsFolder, prefix_File + "-log.xml");
        //        //if (!apiConfig.impersonate)
        //            StaticLogger.Log(TargetType.Xml, ev, fullFileName, string.Empty);


        //        //using (var impersonation = new ImpersonateUser(apiConfig.konectaFolderSettings.user, apiConfig.domain, apiConfig.serviceConfig.konectaFolderSettings.password, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
        //        //{
        //        //    StaticLogger.Log(TargetType.Xml, ev, fullFileName, string.Empty);

        //        //}



        //    }
        //    catch (System.Security.SecurityException ex)
        //    {
        //        //TechnicalException te = new TechnicalException("No hay permisos para escribir en el archivo log", ex);
        //        //te.ErrorId = "2000";
        //        //te.Source = ex.Source;
        //        //throw te;
        //    }
        //    catch (Exception ex)
        //    {

        //    }

        //}
        //public static void Log_Database(Event ev)
        //{
        //    ev.User = Environment.UserName;
        //    ev.Machine = Environment.MachineName;
        //    ev.LogDate = System.DateTime.Now;


        //            ev.AppId = wapiAppSettings.serviceName;

        //    try
        //    {
        //        StaticLogger.Log(TargetType.Database, ev, string.Empty, connectionStrName);
        //    }
        //    catch (Exception ex)
        //    {
        //        Log_FileSystem(ex);
        //        Log_FileSystem(ev);
        //    }
        //}
        //public static void Log_Database(Exception ex)
        //{
        //    if (ex == null) return;

        //    Event ev = new Event();
        //    ev.LogType = EventType.Error;
        //    ev.Source = ex.Source;


        //    ev.Message.Text = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex);


        //    Log_Database(ev);


        //}

        ///// <summary>
        ///// Intenta almacenar eventos/errores en file system
        ///// </summary>
        ///// <param name="ev"></param>
        //public static void Log_FileSystem(Exception ex)
        //{
        //    if (ex == null) return;

        //    Event ev = new Event();
        //    ev.LogType = EventType.Error;
        //    ev.Source = ex.Source;
        //    ev.User = Environment.UserName;
        //    ev.Machine = Environment.MachineName;
        //    //if (wapiAppSettings.apiConfig != null)
        //    //    if (wapiAppSettings.apiConfig.serviceConfig != null)
        //            ev.AppId = wapiAppSettings.serviceName;
        //        //else
        //        //    ev.AppId = "konecta service : tu recibo web";
        //    ev.Message.Text = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex);
        //    ev.LogDate = System.DateTime.Now;
        //    Log_FileSystem(ev);

        //}
        //public static void Log_FileSystem(string source, string msg)
        //{

        //    Event ev = new Event();
        //    ev.LogType = EventType.Information;
        //    ev.Source = string.IsNullOrEmpty(source) ? wapiAppSettings.serviceName : source;
        //    ev.User = Environment.UserName;
        //    ev.Machine = Environment.MachineName;
        //    ev.AppId = wapiAppSettings.serviceName;
        //    ev.Message.Text = msg;
        //    ev.LogDate = System.DateTime.Now;
        //    Log_FileSystem(ev);

        //}
        //static Event GetEcentFromException(Exception ex)
        //{

        //    Event ev = new Event();
        //    ev.LogType = EventType.Error;
        //    ev.Source = ex.Source;
        //    ev.User = Environment.UserName;
        //    ev.Machine = Environment.MachineName;
        //    ev.AppId = wapiAppSettings.serviceName;
        //    ev.Message.Text = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex);
        //    ev.LogDate = System.DateTime.Now;

        //    return ev;
        //}

        #endregion


        /// <summary>
        /// Converts a DateTime to the long representation which is the number of seconds since the unix epoch.
        /// Epoch (UNIX Epoch time) : It is the number of seconds that have elapsed since 00:00:00 Thursday, 1 January 1970,[2] Coordinated Universal Time (UTC), minus leap seconds. 
        /// </summary>
        /// <param name="dateTime">A DateTime to convert to epoch time.</param>
        /// <returns>The long number of seconds since the unix epoch.</returns>
        public static long ToEpoch(DateTime dateTime)
        {
            return (long)(dateTime - new DateTime(1970, 1, 1)).TotalMilliseconds;
        }


        /// <summary>
        /// Converts a long representation of time since the unix epoch to a DateTime.
        /// </summary>
        /// <param name="epoch">The number of seconds since Jan 1, 1970.
        /// Epoch (UNIX Epoch time) : It is the number of seconds that have elapsed since 00:00:00 Thursday, 1 January 1970,[2] Coordinated Universal Time (UTC), minus leap seconds. </param>
        /// <returns>A DateTime representing the time since the epoch.</returns>
        public static DateTime FromEpoch(long epoch)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified).AddMilliseconds(epoch);
        }



    }
}
