using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace epironApi.webApi.Models
{
    public class ApiServerInfo
    {
        public string HostName { get; set; }
        public string SQLServerepiron { get; set; }
        public string SQLServerSeguridad { get; set; }
        public string Ip { get; set; }
    }
}