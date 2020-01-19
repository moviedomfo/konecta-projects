using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{
    public class AuthenticationTypeBE
    {
        public int AuthenticationTypeId { get; set; }
        public string AuthenticationTypeName { get; set; }
        //public string AuthenticationTypeActiveFlag { get; set; }
        //AuthenticationTypeCreated{ get; set; }
        public Guid AuthenticationTypeGUID { get; set; }
        //AuthenticationTypeModifiedDate{ get; set; }
        //AuthenticationTypeModifiedByUser{ get; set; }
        //UserName{ get; set; }
        public string AuthenticationTypeTag { get; set; }
    }
}