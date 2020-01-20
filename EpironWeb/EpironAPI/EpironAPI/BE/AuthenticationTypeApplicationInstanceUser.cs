using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{
    public class AuthenticationTypeApplicationInstanceUserBE
    {
        public Guid ATAUGUID { get; set; }
        public Guid UserGUID { get; set; }
        public Boolean AuthenticationTypeUserMustChangePassword { get; set; }
    }
}