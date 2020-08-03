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
using System.Net.Mail;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CentralizedSecurity.webApi
{
    public class Common
    {
        public const string changePwd = "C";
        public const string changePwdSelft = "A";
        public const string resetear = "R";
        public const string desbloquear = "D";
        public const string CnnStringNameAD = "AD";
        public static string CnnStringAD = string.Empty;
        public const string CnnStringNameMeucci = "meucci";
        public static string CnnStringMeucci = string.Empty;
        static ISymetriCypher ISymetriCypher;
        static string SEED_K = "SESshxdRu3p4ik3IOxM6/qAWmmTYUw8N1ZGIh1Pgh2w=$pQgQvA49Cmwn8s7xRUxHmA==";
        public static List<DomainUrlInfo> DomainUrlInfoList;
        public static List<DomainsBE> Domains;
        public static int ExpirationSeconds = 600;
        public static bool mustChangedNextLogon = false;
        static DateTime expirationDate = new DateTime();
        static Common()
        {
            expirationDate = DateTime.Now.AddSeconds(ExpirationSeconds);
            //string domains = "";
            //-1 = This value does the reverse of 0.It makes the password not expired
            if (System.Configuration.ConfigurationManager.AppSettings["mustChangedNextLogon"] != null)
                mustChangedNextLogon = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["mustChangedNextLogon"]);

             ISymetriCypher = new SymetriCypher<RijndaelManaged>(SEED_K); 

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
                DomainUrlInfoList = Fwk.Security.ActiveDirectory.DirectoryServicesBase.DomainsUrl_Get_FromSp_all(Common.CnnStringNameAD);
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
                throw new TechnicalException(ex.Message);// "El archivo " + domains + " No tiene un formato correcto");
            }


        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Encrypt(string data)
        {
            return ISymetriCypher.Encrypt(data);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static string Dencrypt(string cryptedData)
        {
            return ISymetriCypher.Dencrypt(cryptedData);
        }

       

        public static bool IsEncrypted(System.Configuration.Configuration config)
        {
            if (config.AppSettings.Settings["crypt"] == null)
                return false;
            else
                return Convert.ToBoolean(config.AppSettings.Settings["crypt"].Value);
        }
        internal static bool IsEncrypted()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["crypt"] == null)
                return false;
            else
                return Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["crypt"]);
        }

        internal static SqlConnection GetCnn(string cnnName)
        {
            if (System.Configuration.ConfigurationManager.ConnectionStrings[cnnName] == null)
            {
                throw new Fwk.Exceptions.TechnicalException("Falta cadena de conexion " + cnnName + " en el server");
            }
            System.Data.SqlClient.SqlConnection cnn = null;
            if (IsEncrypted())
            {
                cnn = new System.Data.SqlClient.SqlConnection(ISymetriCypher.Dencrypt(System.Configuration.ConfigurationManager.ConnectionStrings[cnnName].ConnectionString));
            }
            else
            {
                cnn = new System.Data.SqlClient.SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings[cnnName].ConnectionString);
            }

            return cnn;
        }

        internal static SqlConnectionStringBuilder Get_SqlConnectionStringBuilder(string cnnName)
        {

            var cnnString = string.Empty;
            if (IsEncrypted())
            {
                cnnString = ISymetriCypher.Dencrypt(System.Configuration.ConfigurationManager.ConnectionStrings[cnnName].ConnectionString);
            }
            else
            {
                cnnString = System.Configuration.ConfigurationManager.ConnectionStrings[cnnName].ConnectionString;
            }

            return new SqlConnectionStringBuilder(cnnString);

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

            if (now > expirationDate)
            {

                expirationDate = DateTime.Now.AddSeconds(ExpirationSeconds);

                return true;
            }
            return false;

        }
        // Hash an input string and return the hash as
        // a 32 character hexadecimal string.
        public static string getMd5Hash(string input)
        {
            // Create a new instance of the MD5CryptoServiceProvider object.
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        // Verify a hash against a string.
        public static bool verifyMd5Hash(string input, string hash)
        {
            // Hash the input.
            string hashOfInput = getMd5Hash(input);
            return (hashOfInput.Equals(hash, StringComparison.OrdinalIgnoreCase));

        }


        /// <summary>
        /// Envia mail de acuerdo a las direcciones configuradas.
        /// </summary>
        public static void SendMail(string subjet, string body,  string to, string accountConfig)
        {
        

            //Crea el nuevo correo electronico con el cuerpo del mensaje y el asutno.   
            MailMessage wMailMessage = new MailMessage() { Body = body, Subject = subjet };
            wMailMessage.IsBodyHtml = true;

            #region append image on body
            //// Add image attachment from local disk
            //Attachment oAttachment = new Attachment("img_header_content_id.jpg");
            //// Specifies the attachment as an embedded image contentid can be any string.
            //string contentID = "img_header_content_id";
            //oAttachment.ContentId = contentID;
            //wMailMessage.Attachments.Add(oAttachment);
            #endregion

            //Asigna el remitente del mensaje de acuerdo a direccion obtenida en el archivo de configuracion.
            wMailMessage.From = new MailAddress(apiHelper.apiConfig.api_mail_from);
            //Asigna los destinatarios del mensaje de acuerdo a las direcciones obtenidas en el archivo de configuracion.
            //foreach (string recipient in MailRecipients)
            //{
            wMailMessage.To.Add(new MailAddress(to));
            //}

            //SmtpClient wSmtpClient = new SmtpClient("smtp.gmail.com", 587);
            //wSmtpClient.EnableSsl = true;
            //NetworkCredential cred = new NetworkCredential("celamltda", "celam+123");
            //wSmtpClient.Credentials = cred;


            SmtpClient wSmtpClient = new SmtpClient(apiHelper.apiConfig.api_mail_smpt, apiHelper.apiConfig.api_mail_port);
            wSmtpClient.EnableSsl = apiHelper.apiConfig.api_mail_enableSSL;
            NetworkCredential cred = new NetworkCredential(apiHelper.apiConfig.api_mail_user, apiHelper.apiConfig.api_mail_pwd);
            wSmtpClient.Credentials = cred;


            //Inicializa un nuevo cliente smtp de acuerdo a las configuraciones 
            //obtenidas en la seccion mailSettings del archivo de configuracion.
            //SmtpClient wSmtpClient = new SmtpClient(accountConfig);


            //Envia el correo electronico.
            try
            {

                wSmtpClient.Send(wMailMessage);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static string GetBaseUrl()
        {
            var request = HttpContext.Current.Request;
            var appUrl = HttpRuntime.AppDomainAppVirtualPath;

            if (appUrl != "/")
                appUrl = "/" + appUrl;

            var baseUrl = string.Format("{0}://{1}{2}", request.Url.Scheme, request.Url.Authority, appUrl);

            return baseUrl;
        }

    }


}