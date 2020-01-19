using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.BE
{
    public class UserBE
    {
        public int UserId { get; set; }
        public Guid UserGUID { get; set; }
        
        public string UserName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonFirstName { get; set; }

        public string PersonDocNumber { get; set; }
        public Guid PersonGUID { get; set; }
        public Guid UserPlaceGuid { get; set; }

        public string UserPlaceName { get; set; }
        public string UserPlaceDescript { get; set; }
        public DateTime PersonModifiedDate { get; set; }


    }
}