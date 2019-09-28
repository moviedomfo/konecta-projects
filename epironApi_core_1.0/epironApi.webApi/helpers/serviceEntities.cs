using Fwk.DataBase;
using Fwk.Exceptions;
using Fwk.HelperFunctions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace epironApi.webApi.helpers
{

    /// <summary>
    /// appsettings.json 
    /// </summary>
    public class apiAppSettings
    {
        public static WebProxy proxy { get; set; }

        public static ServerSettings serverSettings { get; set; }


        static apiAppSettings()
        {
            //set_proxy();
        }

        static void set_proxy()
        {

            if (serverSettings.apiConfig.proxyEnabled == false)
                return;

            if (proxy == null)
            {
                var proxyURI = new Uri(string.Format("http://{0}:{1}", apiAppSettings.serverSettings.apiConfig.proxyName, apiAppSettings.serverSettings.apiConfig.proxyPort));
                //ICredentials credentials = new NetworkCredential(wapiHelper.apiConfig.proxyUser, wapiHelper.apiConfig.proxyPassword, "allus-ar");
                //proxy = new WebProxy(proxyURI, true, null, credentials);

                proxy = new WebProxy(proxyURI);
                //proxy.Credentials = new System.Net.NetworkCredential("moviedo", "Konecta+45", "allus-ar");
                proxy.Credentials = new System.Net.NetworkCredential(apiAppSettings.serverSettings.apiConfig.proxyUser, apiAppSettings.serverSettings.apiConfig.proxyPassword, apiAppSettings.serverSettings.apiConfig.proxyDomain);

            }

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

        public static HttpResponseMessage getHttpResponseMessage(Exception ex)
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
                        msg = apiAppSettings.serverSettings.apiConfig.bootApiBaseUrl + " no es accesible " + Environment.NewLine + msg;
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
            var resp = new HttpResponseMessage(HttpStatusCode.NotFound)
            {
                Content = new StringContent(msg),
                ReasonPhrase = msg
            };
            return resp;
        }

        /// <summary>
        /// sobrecarga para cargar apiConfig desde una ruta espesifica
        /// </summary>
        /// <param name="path"></param>
        internal static void InitializeConfig(string path)
        {
            try
            {
                //var appS = apiAppSettings.CreateNew(path);

                //serverSettings = new serverSettings();
                //serverSettings.apiConfig = appS.wapiConfig;
                //serverSettings.cnnStrings = get_cnnStrings(apiAppSettings.connectionStrings);
                ////apiHelper.connectionStrings = appS.ConnectionStrings;
                //if (!System.IO.Directory.Exists(serverSettings.apiConfig.logsFolder))
                //    System.IO.Directory.CreateDirectory(serverSettings.apiConfig.logsFolder);

                //setProxy();
            }
            catch (Exception ex)
            {
                //Log_FileSystem(ex);
                throw ex;
            }
        }

        #region no usados 
        /// <summary>
        /// Carga un appsettings.json y lo serializa en <see>wapiAppSettings</see>  
        /// asp net core utiliza este archivo en lugar de un xml.config como en el caso de las aplicaciones .net
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        internal static apiAppSettings CreateNew(string path)
        {

            apiAppSettings settings = null;
            if (string.IsNullOrEmpty(path))
            {
                //wapiConfigPath = System.Configuration.ConfigurationManager.AppSettings.Get("wapiConfig");

                if (String.IsNullOrEmpty(path))
                    throw new TechnicalException("No se encuentra configurada la ruta del archivo  el wapiConfig.json en web.config settings");

            }

            if (System.IO.File.Exists(path) == false)
                throw new TechnicalException("No existe el archivo  " + path);


            string apiConfigJson = FileFunctions.OpenTextFile(path);
            settings = (apiAppSettings)SerializationFunctions.DeSerializeObjectFromJson(typeof(apiAppSettings), apiConfigJson);



            return settings;
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
                    serverSettings.apiConfig = config;

                    //apiConfig.logsFolder = @"c:\wapi_logs";
                    if (!System.IO.Directory.Exists(config.logsFolder))
                        System.IO.Directory.CreateDirectory(config.logsFolder);

                    //setProxy();
                }



            }
            catch (Exception ex)
            {
                //Log_FileSystem(ex);
                throw ex;
            }
        }

        #endregion

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
            var cn = apiAppSettings.serverSettings.connectionStrings.Where(c => c.name.Equals(cnnStringName)).FirstOrDefault();
            if (cn != null)
            {
                return cn as ConnectionString;
            }
            else
            {
                return null;
            }


        }


        //public static void setProxy()
        //{
        //    if (apiAppSettings.serverSettings.apiConfig.proxyEnabled)
        //    {
        //        var proxyURI = new Uri(string.Format("http://{0}:{1}", apiAppSettings.serverSettings.apiConfig.proxyName, apiAppSettings.serverSettings.apiConfig.proxyPort));
        //        proxy = new HttpClientHandler
        //        {
        //            Proxy = new WebProxy(proxyURI, false),
        //            UseProxy = true,
        //            Credentials = new NetworkCredential(apiAppSettings.serverSettings.apiConfig.proxyUser, apiAppSettings.serverSettings.apiConfig.proxyPassword,
        //            apiAppSettings.serverSettings.apiConfig.proxyDomain)
        //        };
        //    }
        //}

    }

    public class ServerSettings
    {
        public ConnectionStrings connectionStrings { get; set; }
        //public cnnStrings cnnStrings { get; set; }
        public apiConfig apiConfig { get; set; }
    }



    public class apiConfig
    {

        /// <summary>
        /// si wapiConfigPath = "" intenta buscarlo en AppSettings si no lo encuntra configurado lo buscara en el root de la aplicacion
        /// </summary>
        /// <param name="wapiConfigPath"></param>
        /// <returns></returns>
        public static apiConfig CreateNew(string wapiConfigPath)
        {

            apiConfig apiConfig = null;
            if (string.IsNullOrEmpty(wapiConfigPath))
            {
                //wapiConfigPath = System.Configuration.ConfigurationManager.AppSettings.Get("wapiConfig");

                if (String.IsNullOrEmpty(wapiConfigPath))
                    throw new TechnicalException("No se encuentra configurada la ruta del archivo  el wapiConfig.json en web.config settings");

            }

            if (System.IO.File.Exists(wapiConfigPath) == false)
                throw new TechnicalException("No existe el archivo  " + wapiConfigPath);


            string apiConfigJson = FileFunctions.OpenTextFile(wapiConfigPath);
            apiConfig = (apiConfig)SerializationFunctions.DeSerializeObjectFromJson(typeof(apiConfig), apiConfigJson);



            return apiConfig;
        }

        public string api_secretKey { get; set; }
        public string api_audienceToken { get; set; }
        public string api_issuerToken { get; set; }
        public string api_expireTime { get; set; }
        public string api_authServerBaseUrl { get; set; }
        
        public string proxyPort { get; set; }
        public string proxyName { get; set; }
        public bool proxyEnabled { get; set; }
        public string proxyUser { get; set; }
        public string proxyPassword { get; set; }
        public string logsFolder { get; set; }


        public string proxyDomain { get; set; }


        public string bootApiBaseUrl { get; set; }
    }



    public class cnnStrings : List<cnnString>
    {

    }
    public class cnnString
    {
        public string name { get; set; }
        public string databaseName { get; set; }
        public string serverName { get; set; }
        public string userName { get; set; }

        public bool windowsAutentification { get; set; }
    }




    public class ConnectionStrings : List<ConnectionString>
    {

    }

    public class ConnectionString
    {
        public string name { get; set; }
        public string cnnString { get; set; }
    }


}
