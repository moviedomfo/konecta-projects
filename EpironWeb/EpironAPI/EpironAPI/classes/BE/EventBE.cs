using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{
    public class EventBE
    {

        
        public int EventId { get; set; }
        public Guid EventGUID { get; set; }
        public string EventName { get; set; }
        
        public int EventTypeId { get; set; }
        public int EventDurationTime { get; set; }


        public int EventRetriesQuantity { get; set; }
        public string EventTag { get; set; }


      

        public DateTime auditTrailLoginEndDate 
        {
            get => DateTime.Now.AddSeconds(this.EventDurationTime);
        }
    }
}