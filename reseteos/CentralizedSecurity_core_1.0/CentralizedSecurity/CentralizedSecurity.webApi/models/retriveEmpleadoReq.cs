using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralizedSecurity.webApi.Models
{
    public class retriveEmpleadosReseteosReq
    {
        public bool userCAIS { get; set; }
        public string dni { get; set; }
        //public string nroTicket { get; set; }
        public string domain { get; set; }
        
        public string userName { get; set; }
    }

  
}
    
    