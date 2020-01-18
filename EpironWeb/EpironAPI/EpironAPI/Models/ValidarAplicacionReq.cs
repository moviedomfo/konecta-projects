using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.Models
{
    public class ValidarAplicacionReq
    {
        public string Event_Tag { get; set; }
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

        public string AutTypeGUID { get; set; }


        public string UserName { get; set; }
        public string UserKey { get; set; }
        public Guid DomainGUID { get; set; }
        public Guid guidintercambio { get; set; }
    }
}