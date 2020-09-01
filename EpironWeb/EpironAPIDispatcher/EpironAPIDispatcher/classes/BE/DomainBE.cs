using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{

    


    public class DomainBE
    {
        public Int32 DomainId { get; set; }
        public string DomainName { get; set; }
        public string LDAPPath { get; set; }
        public string DomainUsr { get; set; }
        public string DomaiDomainDN { get; set; }
        public string DomainSiteName { get; set; }
        public Guid DomainGuid { get; set; }
        
        public string DomainPwd{ get; set; }




    }
}