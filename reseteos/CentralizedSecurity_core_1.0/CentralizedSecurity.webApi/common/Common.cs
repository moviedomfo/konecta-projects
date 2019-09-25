using CentralizedSecurity.webApi.helpers;
using CentralizedSecurity.webApi.Models;
using Fwk.Exceptions;
using Fwk.HelperFunctions;
using Fwk.Security.ActiveDirectory;
using Fwk.Security.Cryptography;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;

namespace CentralizedSecurity.webApi.common
{
    public class Common
    {
        public const string resetear = "R";
        public const string desbloquear = "D";
        public const string CnnStringNameAD = "AD";
        public static string CnnStringAD = string.Empty;
        public const string CnnStringNameMeucci = "meucci";
        public static string CnnStringMeucci = string.Empty;
        internal static ISymetriCypher ISymetriCypher;
        public static List<DomainUrlInfo> DomainUrlInfoList;
        public static List<DomainsBE> Domains;
        public static int ExpirationSeconds=600;
        static DateTime expirationDate = new DateTime();

        static Common()
        {
            expirationDate = DateTime.Now.AddSeconds(ExpirationSeconds);
            //string domains = "";
            //if (System.Configuration.ConfigurationManager.AppSettings["domains"] != null)
            //    domains = System.Configuration.ConfigurationManager.AppSettings["domains"].ToString();



            //if (domains == null)
            //{
            //    throw new TechnicalException("No se encontro configurada la appSetting secConfig ");
            //}
            //if (!System.IO.File.Exists(domains))
            //{
            //    throw new TechnicalException("No se encontro el archivo " + domains);
            //}

            //string domainsJson = FileFunctions.OpenTextFile(domains);

         


            try
            {
                var cnn = Common.GetCnn(Common.CnnStringNameAD);
                DomainUrlInfoList = Fwk.Security.ActiveDirectory.DirectoryServicesBase.DomainsUrl_Get_FromSp_all(cnn.ConnectionString);
                //Domains = new List<DomainsBE>();
                //DomainsBE dbe =null;
                //DomainUrlInfoList.ForEach(d =>
                //{
                //    dbe = new DomainsBE();
                //    dbe.Domain = d.SiteName;
                //    Domains.Add(dbe);

                //});
                //Domains = (List<DomainsBE>)SerializationFunctions.DeSerializeObjectFromJson(typeof(List<DomainsBE>), domainsJson);

                
            }
            catch (Exception ex)
            {
                throw new TechnicalException(ex.Message) ;// "El archivo " + domains + " No tiene un formato correcto");
            }


        }



        //public static bool IsEncrypted(System.Configuration.Configuration config)
        //{
        //    if (config.AppSettings.Settings["crypt"] == null)
        //        return false;
        //    else
        //        return Convert.ToBoolean(config.AppSettings.Settings["crypt"].Value);
        //}

        internal static bool IsEncrypted()
        {
            //if (System.Configuration.ConfigurationManager.AppSettings["crypt"] == null)
                return false;
            //else
            //    return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["crypt"]);
        }

        internal static SqlConnection GetCnn(string cnnName)
        {
            
            if (apiAppSettings.get_cnnString_byName(cnnName) == null) {
                throw new Fwk.Exceptions.TechnicalException("Falta cadena de conexion " + cnnName + " en el server");
            }
            System.Data.SqlClient.SqlConnection cnn = null;
            if (IsEncrypted())
            {
                cnn = new System.Data.SqlClient.SqlConnection(ISymetriCypher.Dencrypt(apiAppSettings.get_cnnString_byName(cnnName).cnnString));
            }
            else
            {
                cnn = new System.Data.SqlClient.SqlConnection(apiAppSettings.get_cnnString_byName(cnnName).cnnString);
            }

            return cnn;
        }

        internal static SqlConnectionStringBuilder Get_SqlConnectionStringBuilder(string cnnName)
        {

            var cnnString = string.Empty;
            if (IsEncrypted())
            {
                cnnString = ISymetriCypher.Dencrypt(apiAppSettings.get_cnnString_byName(cnnName).cnnString);
            }
            else
            {
                cnnString = apiAppSettings.get_cnnString_byName(cnnName).cnnString;
            }

            return  new SqlConnectionStringBuilder(cnnString);

        }
        

        internal static string Get_IPAddress()
        {
            IPAddress iPAddress = null;

            foreach (var a in Dns.GetHostEntry(Dns.GetHostName()).AddressList)
            {
                if (IPAddress.Parse(a.ToString()).AddressFamily == AddressFamily.InterNetwork)
                    iPAddress = a;
            }

            return iPAddress.ToString();
          
        }


        /// <summary>
        /// Retorna si expiro el tiempo de cacheo
        /// </summary>
        internal static bool Expired_ttl()
        {
            
            DateTime now = DateTime.Now;

            if(now > expirationDate)
            {

                expirationDate = DateTime.Now.AddSeconds(ExpirationSeconds);

                return true;
            }
            return false;
           
        }

}

  
}