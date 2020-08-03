using Fwk.Exceptions;
using Fwk.HelperFunctions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralizedSecurity.webApi
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
                wapiConfigPath = System.Configuration.ConfigurationManager.AppSettings.Get("apiConfig");

                if (String.IsNullOrEmpty(wapiConfigPath))
                    throw new TechnicalException("No se encuentra configurada la ruta del archivo el apiConfig.json en web.config settings");

            }

            if (System.IO.File.Exists(wapiConfigPath) == false)
                throw new TechnicalException("No existe el archivo  " + wapiConfigPath);


            string apiConfigJson = FileFunctions.OpenTextFile(wapiConfigPath);
            apiConfig = (apiConfig)SerializationFunctions.DeSerializeObjectFromJson(typeof(apiConfig), apiConfigJson);



            return apiConfig;
        }


        public string api_mail_from { get; set; }
        public string api_mail_user { get; set; }
        public string api_mail_smpt { get; set; }
        public int api_mail_port { get; set; }
        public string api_mail_pwd { get; set; }
        public bool api_mail_enableSSL { get; set; }

        public string url_reseteoBase { get; set; }



        public string proxyPort { get; set; }
        public string proxyName { get; set; }
        public bool proxyEnabled { get; set; }
        public string proxyUser { get; set; }
        public string proxyPassword { get; set; }
        public string logsFolder { get; set; }


        
    }
}