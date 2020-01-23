using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.Models
{
    public class ExecuteReq
    {
        public string serviceProviderName { get; set; }
        public string serviceName { get; set; }
        public object jsonRequest { get; set; }
    }

    public class WhiteListSVC
    {
        public List<WhiteListItem> whiteList { get; set; }
    }
    public class WhiteListItem
    {
        public string serviceName { get; set; }
    }
}