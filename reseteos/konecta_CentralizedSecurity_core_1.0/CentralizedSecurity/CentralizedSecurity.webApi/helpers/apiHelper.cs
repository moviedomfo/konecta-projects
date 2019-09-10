
using Fwk.DataBase;
using Fwk.HelperFunctions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace CentralizedSecurity.webApi.helpers
{
    public class apiHelper
    {
        public static HttpClientHandler proxy { get; set; }


        public static serverSettings serverSettings = null;
        public static List<ConnectionString> connectionStrings;
        public static string serviceName = "konecta_wapi";
        static apiHelper()
        {

            //InitializeConfig();

        }

        /// <summary>
        /// sobrecarga para cargar apiConfig desde una ruta espesifica
        /// </summary>
        /// <param name="path"></param>
        internal static void InitializeConfig(string path)
        {
            try
            {
                var appS = apiAppSettings.CreateNew(path);

                serverSettings = new serverSettings();
                serverSettings.apiConfig = appS.wapiConfig;
                serverSettings.cnnStrings = get_cnnStrings(appS.ConnectionStrings);
                apiHelper.connectionStrings = appS.ConnectionStrings;
                if (!System.IO.Directory.Exists(serverSettings.apiConfig.logsFolder))
                    System.IO.Directory.CreateDirectory(serverSettings.apiConfig.logsFolder);

                setProxy();
            }
            catch (Exception ex)
            {
                //Log_FileSystem(ex);
                throw ex;
            }
        }

        /// <summary>
        /// sobrecarga para cargar apiconfig desde otro lugar
        /// </summary>
        /// <param name="config"></param>
        public static void InitializeConfig(apiAppSettings config)
        {
            try
            {
                serverSettings = new serverSettings();
                serverSettings.apiConfig = config.wapiConfig;
                serverSettings.cnnStrings = get_cnnStrings(config.ConnectionStrings);


                apiHelper.connectionStrings = config.ConnectionStrings;
                if (!System.IO.Directory.Exists(serverSettings.apiConfig.logsFolder))
                    System.IO.Directory.CreateDirectory(serverSettings.apiConfig.logsFolder); 

                setProxy();
            }
            catch (Exception ex)
            {
                //Log_FileSystem(ex);
                throw ex;
            }
        }


        public static string getMessageException(Exception ex)
        {
            string msg = string.Empty;
            //if (ex.GetType() == typeof(HttpResponseException))
            //        msg = ex.Message;
            //return msg = ex.Message;


            if (ex.InnerException != null)
            {

                msg = ex.InnerException.Message;
                if (ex.InnerException.GetType() == typeof(System.Net.Sockets.SocketException))
                {
                    var e = ex.InnerException as System.Net.Sockets.SocketException;
                    if (e.ErrorCode == 10060)
                        msg = apiHelper.serverSettings.apiConfig.apiDomain + " no es accesible " + Environment.NewLine + msg;
                }
                //if (ex.InnerException.GetType() == typeof(WebExcepcion))
                //{
                //    var e = ex.InnerException as System.Net.Sockets.SocketException;
                //    if (e.ErrorCode == 10060)
                //        msg = "WAPI wapiHelper.apiConfig.apiDomain no es accesible " + Environment.NewLine + msg;
                //}
            }

            else
                msg = ex.Message;

            return msg;
        }


        public static HttpClientHandler getProxy_HttpClientHandler()
        {
            HttpClientHandler httpClientHandler = null;
            if (serverSettings.apiConfig.proxyEnabled)
            {
                var proxy = new WebProxy()
                {
                    Address = new Uri(string.Format("http://{0}:{1}", apiHelper.serverSettings.apiConfig.proxyName, apiHelper.serverSettings.apiConfig.proxyPort)),
                    //BypassOnLocal = false,
                    UseDefaultCredentials = true

                    // *** These creds are given to the proxy server, not the web server ***
                    //Credentials = new NetworkCredential(
                    //    userName: wapiHelper.apiConfig.proxyUser,
                    //    password: wapiHelper.apiConfig.proxyPassword)
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

        public static void setProxy()
        {
            if (serverSettings.apiConfig.proxyEnabled)
            {
                var proxyURI = new Uri(string.Format("http://{0}:{1}", apiHelper.serverSettings.apiConfig.proxyName, apiHelper.serverSettings.apiConfig.proxyPort));
                proxy = new HttpClientHandler
                {
                    Proxy = new WebProxy(proxyURI, false),
                    UseProxy = true,
                    Credentials = new NetworkCredential(apiHelper.serverSettings.apiConfig.proxyUser, apiHelper.serverSettings.apiConfig.proxyPassword,
                    apiHelper.serverSettings.apiConfig.proxyDomain)
                };
            }
        }

        /// <summary>
        /// update and set current apiConfig
        /// </summary>
        /// <param name="config"></param>
        public static void updateConfig(apiConfig config)
        {
            try
            {
                //TODO : ver updateConfig
                var settingName = "";//System.Configuration.ConfigurationManager.AppSettings.Get("wapiConfig");
                if (!String.IsNullOrEmpty(settingName))
                {
                    //if (System.IO.File.Exists(settingName) == false)
                    //{
                    //    throw new Fwk.Exceptions.TechnicalException("No existe el archivo de config " + settingName);
                    //}


                    var apiConfigString = Newtonsoft.Json.JsonConvert.SerializeObject(config, Formatting.Indented, new JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });

                    FileFunctions.SaveTextFile(settingName, apiConfigString, false);
                    apiHelper.serverSettings.apiConfig = config;

                    //apiConfig.logsFolder = @"c:\wapi_logs";
                    if (!System.IO.Directory.Exists(config.logsFolder))
                        System.IO.Directory.CreateDirectory(config.logsFolder);

                    setProxy();
                }



            }
            catch (Exception ex)
            {
                //Log_FileSystem(ex);
                throw ex;
            }
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


        //            ev.AppId = wapiHelper.serviceName;

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
        //    //if (wapiHelper.apiConfig != null)
        //    //    if (wapiHelper.apiConfig.serviceConfig != null)
        //            ev.AppId = wapiHelper.serviceName;
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
        //    ev.Source = string.IsNullOrEmpty(source) ? wapiHelper.serviceName : source;
        //    ev.User = Environment.UserName;
        //    ev.Machine = Environment.MachineName;
        //    ev.AppId = wapiHelper.serviceName;
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
        //    ev.AppId = wapiHelper.serviceName;
        //    ev.Message.Text = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex);
        //    ev.LogDate = System.DateTime.Now;

        //    return ev;
        //}

        public static cnnStrings get_cnnStrings(List<ConnectionString> connectionStrings)
        {

            cnnStrings list = new cnnStrings();


            foreach (ConnectionString c in connectionStrings)
            {
                CnnString sqlBuilder = new CnnString(c.name, c.cnnString);

                if (!string.IsNullOrEmpty(sqlBuilder.InitialCatalog))
                {
                    cnnString cnnString = new cnnString();
                    cnnString.name = c.name;
                    cnnString.serverName = sqlBuilder.DataSource;
                    cnnString.databaseName = sqlBuilder.InitialCatalog;
                    cnnString.userName = sqlBuilder.User;
                    cnnString.windowsAutentification = sqlBuilder.WindowsAutentification;

                    list.Add(cnnString);
                }

            }
            return list;
        }
        

        /// <summary>
        /// 
        /// </summary>
        /// <param name="cnnStringName"></param>
        /// <returns></returns>
        public static ConnectionString get_cnnString_byName(string cnnStringName)
        {
            var cn = connectionStrings.Where(c => c.name.Equals(cnnStringName));
            if (cn != null)
            {
                return cn as ConnectionString;
            }
            else
            {
                return null;
            }


        }
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
