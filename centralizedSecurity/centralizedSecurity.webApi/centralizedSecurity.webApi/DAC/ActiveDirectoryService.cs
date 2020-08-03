using System;
using System.Collections.Generic;
using System.Linq;
using Fwk.Security.ActiveDirectory;
using Fwk.Exceptions;
using Fwk.CentralizedSecurity.Contracts;
using CentralizedSecurity.webApi.helpers;
using CentralizedSecurity.webApi;
using System.DirectoryServices.AccountManagement;
using System.DirectoryServices;
using CentralizedSecurity.webApi.Models;
using System.Text;
using CentralizedSecurity.webApi.DAC;
using System.Web;

namespace Fwk.CentralizedSecurity.helpers
{
    /// <summary>
    /// 
    /// </summary>
    public class ActiveDirectoryService
    {
        
        internal static bool performCustomWindowsContextImpersonalization = false;
        /// <summary>
        /// 
        /// </summary>
        static ActiveDirectoryService()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["FwkImpersonate"] != null)
                performCustomWindowsContextImpersonalization = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["FwkImpersonate"]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal static LoogonUserResult User_Logon(string userName, string password, string domain)
        {
            LoogonUserResult loogonUserResult = new LoogonUserResult();
            loogonUserResult.Autenticated = false;
         
                LDAPHelper _ADWrapper = new LDAPHelper(domain, Common.CnnStringNameAD, true, false);
                TechnicalException logError = null;

                loogonUserResult.LogResult = _ADWrapper.User_Logon(userName, password, out logError).ToString();

                if (logError != null)
                    loogonUserResult.ErrorMessage = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(logError);
                else
                {
                    loogonUserResult.ErrorMessage = string.Empty;
                    loogonUserResult.Autenticated = true;
                }

       
                return loogonUserResult;
            

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="password"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        internal static LoogonUserResult User_Logon2(string userName, string password, string domain)
        {

            LoogonUserResult loogonUserResult = new LoogonUserResult();
            loogonUserResult.Autenticated = false;
            try
            {
                LDAPHelper _ADWrapper = new LDAPHelper(domain, Common.CnnStringNameAD);
                TechnicalException logError = null;

                loogonUserResult.LogResult = _ADWrapper.User_Logon(userName, password, out logError).ToString();

                if (logError != null)
                    loogonUserResult.ErrorMessage = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(logError);
                else
                {
                    loogonUserResult.ErrorMessage = string.Empty;
                    loogonUserResult.Autenticated = true;
                }
              
                return loogonUserResult;


            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        internal static List<ActiveDirectoryGroup> GetGroupsFromUser(string userName, string domain)
        {

            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD);

            List<ADGroup> list = ad.User_SearchGroupList(userName);
            ad.Dispose();
            if (list != null && list.Count != 0)
            {
                var activeDirectoryGroupList = from g in list select new ActiveDirectoryGroup(g);

                return activeDirectoryGroupList.ToList();
            }
            else
                return null;
        }


        internal static List<ActiveDirectoryUser> GetUsersFromGroup(string groupName, string domain)
        {
            try {
                ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD);

                List<ADUser> list = ad.Users_SearchByGroupName(groupName);

                ad.Dispose();
                if (list.Count != 0)
                {
                    var userList = from u in list select new ActiveDirectoryUser(u);

                    return userList.ToList();

                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        internal static Boolean UserExist(string userName, string domain)
        {
           
                ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);

                bool exist = ad.User_Exists(userName);

                ad.Dispose();

               
                return exist;

          
        }
    

        internal static List<ActiveDirectoryGroup> GetGroups(string domain)
        {
        

            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);

            List<ADGroup> list = ad.Groups_GetAll();

            ad.Dispose();

            if (list.Count != 0)
            {
                var userList = from u in list select new ActiveDirectoryGroup(u);

                return userList.ToList<ActiveDirectoryGroup>();
            }
            else
                return null;
        }



        internal static void User_Unlock(string userName, string domain)

        {

            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);
            try
            {
                UnlockAccount2(userName, ad);
            }
            catch (Exception y)
            {

                if (performCustomWindowsContextImpersonalization)
                {
                    using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                    {
                        ad.User_Unlock(userName);
                    }
                }

                else
                {
                    ad.User_Unlock(userName);
                }


                ad.Dispose();
            }

        }

       
        

        internal static void User_Lock(string userName, string domain)
        {
            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, false);


            if (performCustomWindowsContextImpersonalization)
            {
                using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                {
                    ad.User_SetLockedStatus(userName, true);
                }
            }
            else
            {
                ad.User_SetLockedStatus(userName, true);
            }
            
        }


        internal static ActiveDirectoryUser User_Info(string userName, string domain)
        {
            
            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);

            ADUser usr = ad.User_Get_ByName(userName);
            if (usr == null)
                return null;
            return new ActiveDirectoryUser(usr);
        }

        internal static void User_SetActivation(string userName, bool disabled, string domain)
        {
            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, false);

           
            if (performCustomWindowsContextImpersonalization)
            {
                using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                {
                    ad.User_SetActivation(userName, disabled);
                }
            }
            else
            {
                ad.User_SetActivation(userName, disabled);
            }

            ad.Dispose();
        }

        /// <summary>
        /// Intenta con el método ResetPassword2 y si falla utiliza ResetPassword
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="newPassword"></param>
        /// <param name="domain"></param>
        internal static void User_Reset_Password(string userName, string newPassword, string domain)
        {
            try
            {
                ResetPassword(userName, domain, newPassword, true,  Common.mustChangedNextLogon);
            }
            catch (Exception y)
            {
                //userDirectoryEntry.Properties["pwdLastSet"].Value = 
                //pwdLastSet = 0  must be changed at the next logon.
                // -1  = This value does the reverse of 0.It makes the password not expired
                // 
                int flagmustChangedNextLogon = -1;
                if (Common.mustChangedNextLogon) 
                    flagmustChangedNextLogon = 0;

                ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);
                //ADWrapper ad3 = new ADWrapper(domain,"reseteos","*R3s3t30s+");

                if (performCustomWindowsContextImpersonalization)
                {
                    using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                    {
                        ad.User_ResetPwd(userName, newPassword, true, flagmustChangedNextLogon);
                    }
                }
                else
                {
                    ad.User_ResetPwd(userName, newPassword, true, flagmustChangedNextLogon);
                }

                ad.Dispose();
            }
            
        }

        /// <summary>
        /// El usuario ya se encuentra en lapagina de Olvide Contras.. y preciona el boton chequear DNI
        /// 
        /// </summary>
        /// <param name="dni">retorna Empleado con sus usuarios</param>
        /// <returns></returns>
        internal static EmpleadoBE ForgotPassword_checkDNI(string dni)
        {
            StringBuilder str = new StringBuilder("Con el DNI ingresado no podemos ayudarte. Comunícate con CAIS. ");
            str.AppendLine("CANALES DE ATENCIÓN:");
            str.AppendLine("Chat(Incidentes particulares): caischat.grupokonecta.com.ar");
            str.AppendLine("Telefónico(Incidentes Masivos) 54 9 351 4266616");
            str.AppendLine("Mail: cais_argentina@grupokonecta.com");
            try
            {

                EmpleadoBE empleadoBE = MeucciDAC.VirifyUser_ForgotPassword(dni); 

                if (empleadoBE == null)
                {

                    //throw new FunctionalException(1001,"El DNI no se encuentra registrado en nuestras Bases de Datos, verifícalo e intenta nuevamente o comunicarse con CAIS cais_argentina@grupokonecta.com y contacto 3514266616 .- ");
                 
                    throw new FunctionalException(1001, str.ToString());
                }
                if (string.IsNullOrEmpty( empleadoBE.Email))
                {
                    

                    throw new FunctionalException(1000, str.ToString());
                }
                return empleadoBE;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }



        }

        /// <summary>
        /// Solo para propósitos de test
        /// </summary>
        /// <returns></returns>
        EmpleadoBE empleado_mock()
        {  
            //Buscar empleado
            EmpleadoBE empleadoBE = new EmpleadoBE();
            empleadoBE.Email = "marcelo.oviedo@gmail.com";
            empleadoBE.ApeNom = "MOF";
            empleadoBE.WindosUserList = new List<WindosUserBE>();

            WindosUserBE wu = new WindosUserBE();

            wu.WindowsUser = "user1";
            wu.dom_id = 1;
            wu.Dominio = "dominio1";

            empleadoBE.WindosUserList.Add(wu);

            wu = new WindosUserBE();

            wu.WindowsUser = "user2";
            wu.dom_id = 2;
            wu.Dominio = "dominio2";

            empleadoBE.WindosUserList.Add(wu);
            return empleadoBE;
        }


        /// <summary>
        /// El usuario solicita reestablecer contraseña 
        /// </summary>
        /// <param name="dni"></param>
        /// <returns></returns>
        internal static ForgotPasswordRes forgotPassword_requets(string dni)
        {
            ForgotPasswordRes result = new ForgotPasswordRes();
            var baseUrl = Common.GetBaseUrl();

            //verificar si existe en el dominio
            //bool userExst = true;//UserExist(userName, domainName);

            //if (!userExst )
            //{
            //    result.Status = "Error";
            //    result.Message = " <p>  El usuario ingresado no existe.</p>";

            //    return result;
            //}
            EmpleadoBE empleadoBE = MeucciDAC.VirifyUser_ForgotPassword(dni);
            empleadoBE.DNI = dni;
            //Buscar empleado
            //EmpleadoBE empleadoBE = new EmpleadoBE();
            //empleadoBE.email = "marcelo.oviedo@gmail.com";
            //empleadoBE.ApeNom ="MOF";
            //var res = getSocioBEByUserName(userName, false, false);



            if (string.IsNullOrEmpty(empleadoBE.Email))
            {
                result.Status = "Error";
                result.Message = " <p>  El usuario ingresado no registra un correo.</p>";
                  
                return result;
            }

            Int64 ttl = Fwk.HelperFunctions.DateFunctions.DateTimeToUnixTimeStamp(System.DateTime.Now.AddMinutes(10));

            //Generate token
            string toEncrypt = string.Concat(empleadoBE.DNI,";", empleadoBE.Email.Trim(), ";", ttl.ToString());
            string code =  Common.Encrypt(toEncrypt);
            //string code = Common.getMd5Hash(string.Concat(empleadoBE.DNI, empleadoBE.Email.Trim()));

            string file = System.Web.Hosting.HostingEnvironment.MapPath("~/files/Email_Forgot_Password.html");
            try
            {
                string txt = Fwk.HelperFunctions.FileFunctions.OpenTextFile(file);
                StringBuilder BODY = new StringBuilder(txt);

                BODY.Replace("$userName$", empleadoBE.ApeNom);


                //BODY.Replace("$url$", "https://host/selfreset/?code=" + code);


                //string forgotPwd = String.Format("reset/{0}/{1}", code, empleadoBE.DNI);
                string forgotPwd = String.Format("reset?code={0}", HttpUtility.UrlEncode(code));
               
                BODY.Replace("$url$", apiHelper.apiConfig.url_reseteoBase + forgotPwd);
                
                
                Common.SendMail(string.Concat("Solicitud de cambio de contraseña"), BODY.ToString(),  empleadoBE.Email.Trim(), "");

                int at = empleadoBE.Email.IndexOf('@');

                var mail = "*******" + empleadoBE.Email.Substring(at  -3 , empleadoBE.Email.Length - at + 3);


                result.Message = "Le enviaremos un email a su casilla de correo " + mail + " y " +
                    "en el mismo encontrará un código de acceso en un enlace para que finalice el " +
                   "proceso de cambio de contraseña";

                
                result.Status = "Success";
            }
            catch (Exception ex)
            {
                result.Status = "Error";
                result.Message = Fwk.Exceptions.ExceptionHelper.GetAllMessageException(ex,false);
                
            }


            return result;
            // Common.SendMail("Registracion de socio al sitio web de CELAM", string.Format(txt, userName), "support_noreply@celam.com", emai.Trim(), "");

            
        }

        /// <summary>
        /// El usuario recibe un mail con el codigo de autorizacion, entra ala pagina (link ofrecido) y envia nuevo pàssword
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="code"></param>
        /// <param name="domainName"></param>
        /// <param name="newPassword"></param>
        /// <returns></returns>
        //internal static ForgotPasswordRes forgotPassworChangePassword(string userName, string domainName,string code, string newPassword)
        //{
        //    ForgotPasswordRes result = new ForgotPasswordRes();
            

        //    //result = getSocioBEByUserName(userName);

            
        //    var isValid = Common.verifyMd5Hash(string.Concat(userName, domainName), code);
        //    if (isValid)
        //    {
        //        //User_Reset_Password(userName, newPassword, domainName);

        //        result.Status = "OK";
        //        result.Message = "El reseteo se realizó exitosamente.";
        //    }
        //    else
        //    {
        //        result.Status = "Error";
        //        result.Message = "El código de verificación enviado no es válido o no corresponde al socio en cuestión";
        //    }

        //    return result;
        //}

        /// <summary>
        /// El usuario recibe un mail con el codigo de autorizacion, entra ala pagina (link ofrecido) y en el init se chequea validez de url
        /// </summary>
        /// <param name="code"></param>
        /// <returns>EmpleadoBE</returns>
        internal static EmpleadoBE forgotPassworChangePassword_Verify(string code)
        {





            EmpleadoBE empleadoBE = null;
            string decriptedData = "";
            //var isValid = Common.verifyMd5Hash(string.Concat(dni, empleadoBE.Email), code);
            try
            {
                 decriptedData = Common.Dencrypt(code);
            }
            catch (Exception ex)
            {
                throw new Exception("El códido de seguridad es incorrecto-");
            }
            
            bool isValid = false;
            if (decriptedData.Split(';').Length == 3)
            {
                // DNI;Email;TTL;
                string[] splited = decriptedData.Split(';');
                string dni = splited[0].Trim();
                //Obtengo la fecha de exp
                Int64 epocTtl = Convert.ToInt64(splited[2].Trim());
                Int64 epocNow = Fwk.HelperFunctions.DateFunctions.DateTimeToUnixTimeStamp(System.DateTime.Now);
                
                if(epocTtl < epocNow)
                {
                    throw new Exception("El códido de seguridad a caducado, por favor vuelva a solicitar cambio de contraseña.-");
                }


                empleadoBE = MeucciDAC.VirifyUser_ForgotPassword(dni);
                
                if(empleadoBE!=null)
                {
                    empleadoBE.DNI = dni;
                    isValid = splited[1].Trim().CompareTo(empleadoBE.Email.Trim()) == 0;
                }
            }


            if (isValid)
            {
                return empleadoBE;
            }
            else
            {
                throw new Exception("El código de verificación enviado no es válido o no corresponde al socio en cuestión");
            }

           
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="userName"></param>
        /// <param name="mustChange"></param>
        /// <param name="domain"></param>
        public static void User_MustChangePasswordNextLogon(string userName, bool mustChange,string domain)
        {
            if (Common.mustChangedNextLogon == false)
                return;

            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);
            //PrincipalContext pr = new PrincipalContext(ContextType.Domain, domain, "dc=corp,dc=local", username, password);



            if (performCustomWindowsContextImpersonalization)
            {
                using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                {
                    ad.User_MustChangePasswordNextLogon(userName, mustChange);

                }
            }
            else
            {
                 ad.User_MustChangePasswordNextLogon(userName, mustChange);

            }

        }

        /// <summary>
        /// Utiliza PrincipalContext y UserPrincipal
        /// </summary>
        /// <param name="usernameToresset"></param>
        /// <param name="domain"></param>
        /// <param name="newPassword"></param>
        /// <param name="UnlockAccount"></param>
        /// <param name="nextlogon">Usuario debe cambiar su clave en proximo logeo</param>
        /// <returns></returns>
        public static Boolean ResetPassword(string usernameToresset, string domain, string newPassword, Boolean UnlockAccount, bool nextLogon = false)
        {
           
            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);
            //PrincipalContext pr = new PrincipalContext(ContextType.Domain, domain, "dc=corp,dc=local", username, password);
            
            
            
            if (performCustomWindowsContextImpersonalization)
            {
                using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                {
                    return ResetPassword2(usernameToresset, newPassword, ad, nextLogon);

                }
            }
            else
            {
                return ResetPassword2(usernameToresset,newPassword,ad, nextLogon);

            }


            
        }

        /// <summary>
        /// Utiliza PrincipalContext y UserPrincipal
        /// </summary>
        /// <param name="usernameToresset"></param>
        /// <param name="newPassword"></param>
        /// <param name="ad"></param>
        /// <param name="nextlogon">Usuario debe cambiar su clave en proximo logeo</param>
        /// <returns></returns>
        public static Boolean ResetPassword2(string usernameToresset, string newPassword, ADWrapper ad,bool nextlogon = false)
        {
            var uri = new Uri(ad.LDAPPath);
            var Container = uri.Segments[1];
            Boolean flag = false;


            using (PrincipalContext pr = new PrincipalContext(ContextType.Domain, ad.LDAPDomainName, Container, ad.LDAPUser, ad.LDAPPassword))
            using (UserPrincipal user = UserPrincipal.FindByIdentity(pr, usernameToresset))
            {

                if (user == null)
                {
                    throw new Exception("No se pudo encontrar el usuario " +usernameToresset + " en el dominio " + ad.LDAPDomainName);
                }
                if (user != null && user.Enabled == true)
                {

                    //if (UnlockAccount)
                    //{
                    //    user.UnlockAccount();
                    //}
                    user.SetPassword(newPassword);
                    if (nextlogon)
                    {
                        user.ExpirePasswordNow();
                    }
                    
                    user.Save();
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }




            return flag;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="usernameToresset"></param>
        /// <param name="ad"></param>
        /// <returns></returns>
        public static Boolean UnlockAccount2(string usernameToresset,  ADWrapper ad)
        {
            var uri = new Uri(ad.LDAPPath);
            var Container = uri.Segments[1];
            Boolean flag = false;


            using (PrincipalContext pr = new PrincipalContext(ContextType.Domain, ad.LDAPDomainName, Container, ad.LDAPUser, ad.LDAPPassword))
            using (UserPrincipal user = UserPrincipal.FindByIdentity(pr, usernameToresset))
            {

                if (user == null)
                {
                    throw new Exception("No se pudo encontrar el usuario " + usernameToresset + " en el dominio " + ad.LDAPDomainName);
                }
                if (user != null && user.Enabled == true)
                {

                   
                    user.UnlockAccount();
                    
                    
                    //if (NextLogon)
                    //{
                    //    user.ExpirePasswordNow();
                    //}
                    user.Save();
                    flag = true;
                }
                else
                {
                    flag = false;
                }
            }




            return flag;
        }


        internal static Fwk.CentralizedSecurity.Contracts.DomainsUrl[] GetAllDomainsUrl()
        {


            //List<DomainUrlInfo> auxlist = ADWrapper.DomainsUrl_GetList2(System.Configuration.ConfigurationManager.ConnectionStrings[Common.CnnStringNameAD].ConnectionString);
            List<DomainUrlInfo> auxlist = ADWrapper.DomainsUrl_Get_FromSp_all(Common.CnnStringNameAD);

            if (auxlist.Count != 0)
            {
                var list = from d in auxlist select new Fwk.CentralizedSecurity.Contracts.DomainsUrl(d);

                return list.ToArray<Fwk.CentralizedSecurity.Contracts.DomainsUrl>();
            }
            else
                return null;


        }

        /// <summary>
        /// desde la BD o la cache
        /// </summary>
        /// <returns></returns>
        public static List<DomainUrlInfo> RetriveDomainsUrl()
        {
            //si esta nulo o expiro refresca 
            if (Common.DomainUrlInfoList == null || Common.Expired_ttl())
            {
                Common.DomainUrlInfoList =  DirectoryServicesBase.DomainsUrl_Get_FromSp_all(Common.CnnStringNameAD);
            }

            return Common.DomainUrlInfoList;
        }


        /// <summary>
        /// Retorna DomainName que utiliza fwk DirectoryServices
        /// </summary>
        /// <param name="siteName"></param>
        /// <returns></returns>
        public static string Get_correct_DomainName(string siteName)
        {
            siteName= siteName.Replace("-",".");
            var dom = Common.DomainUrlInfoList.Where(p =>
            //p.DomainName.ToUpper().Equals(siteName.ToUpper()) ||
            p.SiteName.ToUpper().Equals(siteName.ToUpper())
            ).FirstOrDefault();

            if(dom == null)
            {
                throw new Exception("El dominio " + siteName + " no se encuentra configurado en la BD de reseteos");
            }
            return dom.DomainName;
        }

        public string GetRandomPassword()
        {

            Random generator = new Random();
            String r = generator.Next(0000, 999).ToString("D3");

            return "Konecta+" + r;
        }

    }
}