using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EpironApi.webApi.BE
{
    public class AccountOutputChannelTypeBE
    {
        public int ElementTypeId { get; set; }
        public bool InputPublic { get; set; }
        public int ElementTypeOutputId { get; set; }
        public bool OutputPublic { get; set; }
        public int PChannelType { get; set; }

    }
}
