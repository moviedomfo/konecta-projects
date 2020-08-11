using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpironApi.webApi.BE
{
    public class ApplicationSettingsBE
    {
        public int SettingId { get; set; }
        public string Description { get; set; }
        public string Value { get; set; }
        public int? AttentionQueueId { get;  set; }
    }
}
