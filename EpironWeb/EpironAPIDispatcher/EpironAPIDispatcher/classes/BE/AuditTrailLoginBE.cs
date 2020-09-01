using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{
    public class AuditTrailLoginBE
    {
        public string AuditTrailLoginId { get; set; }
        public Guid AuditTrailLoginParentGUID { get; set; }
        public Guid AuditTrailLoginAppInstanceGUID { get; set; }
        public DateTime AuditTrailLoginCreated { get; set; }
        public int EventId { get; set; }
        public string EventName { get; set; }
        public string AuditTrailLoginResponse { get; set; }
        public string AuditTrailLoginActiveFlag { get; set; }
        public Guid AuditTrailLoginGUID { get; set; }
        public string AuditTrailLoginHost { get; set; }
        public string AuditTrailLoginIP { get; set; }
        public string AuditTrailLoginRequest { get; set; }
        public DateTime AuditTrailLoginEndDate { get; set; }


        public int? EventResponseInternalCode { get; set; }
    }
}