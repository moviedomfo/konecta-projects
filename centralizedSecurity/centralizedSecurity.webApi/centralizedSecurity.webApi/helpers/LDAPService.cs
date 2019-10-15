using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CentralizedSecurity.webApi.DAC;
using CentralizedSecurity.webApi.Models;
using Fwk.Security.ActiveDirectory;

namespace CentralizedSecurity.webApi.service
{
    public class LDAPService: ILDAPService
    {

        public List<DomainUrlInfo> DomainsUrl { get; set; }

        


        LDAPService()
        {
            //this.Domains = MeucciDAC.RetriveDominios();
            this.DomainsUrl =  Fwk.Security.ActiveDirectory.DirectoryServicesBase.DomainsUrl_Get_FromSp_all(Common.CnnStringNameAD);
        }


        public static LDAPService CreateInstance()
        {
            return new LDAPService(); 
        }

        /// <summary>
        /// Retorna DomainName que utiliza fwk DirectoryServices
        /// </summary>
        /// <param name="anotherDomainNameOrigin"></param>
        /// <returns></returns>
        public  string Get_correct_DomainName(string anotherDomainNameOrigin)
        {
            var dom = this.DomainsUrl.Where(p =>
            p.DomainName.ToUpper().Equals(anotherDomainNameOrigin.ToUpper()) ||
            p.SiteName.ToUpper().Equals(anotherDomainNameOrigin.ToUpper())
            ).FirstOrDefault();
            if(dom==null)
                throw new Fwk.Exceptions.FunctionalException(string.Format("No encontramos el registro del dominio {0} en la base de datos .-", anotherDomainNameOrigin));
            return dom.DomainName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetRandomPassword(){

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
        List<DomainUrlInfo> DomainsUrl { get; set; }

        string GetRandomPassword();
        /// <summary>
        /// Retorna DomainName que utiliza fwk DirectoryServices
        /// </summary>
        /// <param name="anotherDomainNameOrigin"></param>
        /// <returns></returns>
        string Get_correct_DomainName(string anotherDomainNameOrigin);
    }
}