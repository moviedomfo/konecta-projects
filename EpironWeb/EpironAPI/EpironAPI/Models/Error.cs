using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.Models
{
    public class Error
    {
        public int EventResponseId { get; set; }
        public string EventResponseText { get; set; }
        public int EventResponseInternalCode { get; set; }
        public Guid Guid { get; set; }
    }
}