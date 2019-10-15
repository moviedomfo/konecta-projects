using System;
using System.Collections.Generic;
using System.Linq;
using Fwk.Security.ActiveDirectory;
using Fwk.Exceptions;
using Fwk.CentralizedSecurity.Contracts;
using CentralizedSecurity.webApi.helpers;
using CentralizedSecurity.webApi;
using System.DirectoryServices.AccountManagement;

namespace Fwk.CentralizedSecurity.helpers
{
    public class ActiveDirectoryService
    {
        
        internal static bool performCustomWindowsContextImpersonalization = false;
        //string DomainsUrl domainsUrl = null;
        static ActiveDirectoryService()
        {
            if (System.Configuration.ConfigurationManager.AppSettings["FwkImpersonate"] != null)
                performCustomWindowsContextImpersonalization = Convert.ToBoolean(System.Configuration.ConfigurationManager.AppSettings["FwkImpersonate"]);
        }

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
                ResetPassword(userName, domain, newPassword, true, false);
            }
            catch (Exception y)
            {
                ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);
                //ADWrapper ad3 = new ADWrapper(domain,"reseteos","*R3s3t30s+");

                if (performCustomWindowsContextImpersonalization)
                {
                    using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                    {
                        ad.User_ResetPwd(userName, newPassword, true);
                    }
                }
                else
                {
                    ad.User_ResetPwd(userName, newPassword, true);
                }

                ad.Dispose();
            }
            
        }

        /// <summary>
        /// Utiliza PrincipalContext y UserPrincipal
        /// </summary>
        /// <param name="usernameToresset"></param>
        /// <param name="domain"></param>
        /// <param name="newPassword"></param>
        /// <param name="UnlockAccount"></param>
        /// <param name="NextLogon"></param>
        /// <returns></returns>
        public static Boolean ResetPassword(string usernameToresset, string domain, string newPassword, Boolean UnlockAccount, Boolean NextLogon)
        {
           
            ADWrapper ad = new ADWrapper(domain, Common.CnnStringNameAD, performCustomWindowsContextImpersonalization);
            //PrincipalContext pr = new PrincipalContext(ContextType.Domain, domain, "dc=corp,dc=local", username, password);
            
            
            
            if (performCustomWindowsContextImpersonalization)
            {
                using (var impersonation = new ImpersonateUser(ad.LDAPUser, ad.LDAPDomain, ad.LDAPPassword, ImpersonateUser.LOGON32_LOGON_NEW_CREDENTIALS))
                {
                    return ResetPassword2(usernameToresset, newPassword, ad);

                }
            }
            else
            {
                return ResetPassword2(usernameToresset,newPassword,ad);

            }


            
        }

        /// <summary>
        /// Utiliza PrincipalContext y UserPrincipal
        /// </summary>
        /// <param name="usernameToresset"></param>
        /// <param name="newPassword"></param>
        /// <param name="ad"></param>
        /// <returns></returns>
        public static Boolean ResetPassword2(string usernameToresset, string newPassword, ADWrapper ad)
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