using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.Models
{
    public class ExecuteReq
    {
        /// <summary>
        /// Proveedor de metadata: puede ser metadata en servicios json,xml o en BD
        /// </summary>
        public string serviceProviderName { get; set; }

        /// <summary>
        /// Nombre del fwwk service en capa SVC
        /// </summary>
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