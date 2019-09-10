using Fwk.Exceptions;
using Fwk.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CentralizedSecurity.webApi.helpers
{
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

        public string apiDomain { get; set; }
        public string password { get; set; }
        public string api_user { get; set; }
        public string api_password { get; set; }


        public string proxyPort { get; set; }
        public string proxyName { get; set; }
        public bool proxyEnabled { get; set; }
        public string proxyUser { get; set; }
        public string proxyPassword { get; set; }
        public string logsFolder { get; set; }


        public string proxyDomain { get; set; }
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

    public class serverSettings
    {
        public cnnStrings cnnStrings { get; set; }
        public apiConfig apiConfig { get; set; }
    }




    /// <summary>
    /// appsettings.json 
    /// </summary>
    public class apiAppSettings
    {

        /// <summary>
        /// Carga un appsettings.json y lo serializa en <see>wapiAppSettings</see>  
        /// asp net core utiliza este archivo en lugar de un xml.config como en el caso de las aplicaciones .net
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static apiAppSettings CreateNew(string path)
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

        public List<ConnectionString> ConnectionStrings { get; set; }

       

        public apiConfig wapiConfig { get; set; }


    }

    public class ConnectionString
    {
        public string name { get; set; }
        public string cnnString { get; set; }
    }


}
