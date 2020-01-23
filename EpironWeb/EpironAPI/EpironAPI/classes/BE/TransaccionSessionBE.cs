using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{

    /// <summary>
    /// se usa como AuditTrailLogin
    /// </summary>
    public class TransaccionSessionBE
    {
    
        public Guid AuditTrailSessionGUID { get; set; }
        public int AuditTrailSessionId { get; set; }
        public int AuditTrailLoginId { get; set; }
    }
    
}