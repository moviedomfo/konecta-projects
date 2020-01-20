using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{
    public class AuditTrailLoginBE
    {
        public string AuditTrailLoginId { get; set; }
        public string AuditTrailLoginParentGUID { get; set; }
        public string AuditTrailLoginAppInstanceGUID { get; set; }
        public string AuditTrailLoginCreated { get; set; }
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string AuditTrailLoginResponse { get; set; }
        public string AuditTrailLoginActiveFlag { get; set; }
        public string AuditTrailLoginGUID { get; set; }
        public string AuditTrailLoginHost { get; set; }
        public string AuditTrailLoginIP { get; set; }
        public string AuditTrailLoginRequest { get; set; }
        public string AuditTrailLoginEndDate { get; set; }
    }
}