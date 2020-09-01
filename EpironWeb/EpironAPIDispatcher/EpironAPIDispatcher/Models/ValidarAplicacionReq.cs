using Epiron.Security.BE;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.Models
{
    public class ValidarAplicacionRes
    {
        
        public bool ControlEntity { get; set; }
        public string ApplicationName { get; set; }
        public string ApplicationInstanceName { get; set; }
        public Guid Token { get; set; }


        public List<AuthenticationTypeBE> AuthenticationTypes { get; set; }

        public List<DomainBE> Domains { get; set; }

    }
    public class ValidarAplicacionReq
    {
        //public string Event_Tag { get; set; }
        public Guid AppInstanceGUID { get; set; }
        public string LoginHost { get; set; }
        public string LoginIP { get; set; }
    }


    public class UserAutenticacionReq
    {
        public string Event_Tag { get; set; }
        public Guid AppInstanceGUID { get; set; }
        public string LoginHost { get; set; }
        public string LoginIP { get; set; }

        public Guid AutTypeGUID { get; set; }
        public string UserName { get; set; }
        /// <summary>
        /// password
        /// </summary>
        public string UserKey { get; set; }
        public Guid DomainGUID { get; set; }

        /// <summary>
        /// token
        /// </summary>
        public Guid guidintercambio { get; set; }
    }
}