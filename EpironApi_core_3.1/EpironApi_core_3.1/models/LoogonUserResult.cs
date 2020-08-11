using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fwk.epironApi.Contracts
{

    public class LoogonUserResult
    {
        public string ErrorMessage { get; set; }
        public string LogResult { get; set; }
        public bool Autenticated { get; set; }

        public string Token { get; set; }
    }


}
