using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using chatBotEpironApi.webApi.Models;
using Fwk.Security.ActiveDirectory;


namespace chatBotEpironApi.webApi.common
{
    public class LDAPService : ILDAPService
    {
        List<DomainUrlInfo> domainsUrl;
        
    
        
        public void set_DomainsUrl()
        {
            if(this.domainsUrl == null)
               this.domainsUrl = Fwk.Security.ActiveDirectory.DirectoryServicesBase.DomainsUrl_Get_FromSp_all(Common.CnnStringNameAD);

             this.domainsUrl = new  List<DomainUrlInfo>();
        }
       
        /// <summary>
        /// Retorna DomainName que utiliza fwk DirectoryServices
        /// </summary>
        /// <param name="anotherDomainNameOrigin"></param>
        /// <returns></returns>
        public string Get_correct_DomainName(string anotherDomainNameOrigin)
        {
            set_DomainsUrl();
            var dom = this.domainsUrl.Where(p =>
            p.DomainName.ToUpper().Equals(anotherDomainNameOrigin.ToUpper()) ||
            p.SiteName.ToUpper().Equals(anotherDomainNameOrigin.ToUpper())
            ).FirstOrDefault();
            if (dom == null)
                throw new Fwk.Exceptions.FunctionalException(string.Format("No encontramos el registro del dominio {0} en la base de datos .- DomainsUrl", anotherDomainNameOrigin));
            return dom.DomainName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetRandomPassword()
        {

            Random generator = new Random();
            String r = generator.Next(0000, 999).ToString("D3");

            return "Konecta+" + r;
        }

       
    }

    /// <summary>
    /// 
    /// </summary>
    public interface ILDAPService
    {
        /// <summary>
        /// 
        /// </summary>
        void set_DomainsUrl();

        string GetRandomPassword();
        /// <summary>
        /// Retorna DomainName que utiliza fwk DirectoryServices
        /// </summary>
        /// <param name="anotherDomainNameOrigin"></param>
        /// <returns></returns>
        string Get_correct_DomainName(string anotherDomainNameOrigin);
    }
}