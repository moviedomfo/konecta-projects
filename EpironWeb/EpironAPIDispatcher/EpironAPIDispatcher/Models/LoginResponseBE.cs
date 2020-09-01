using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace EpironAPI.Model
{
    public class UserAutenticacionRes 
    {
        public UserAutenticacionRes()
        {

        }
        //public UserAutenticacionRes(HttpStatusCode statusCode ) :base (statusCode)
        //{
           
        //}

        public Guid Token { get; set; }
        public int WsUserId { get; set; }
        public Guid UserGuid { get; set; }
        public string UserName { get; set; }
        public string PersonFirstName { get; set; }
        public string PersonLastName { get; set; }
        public string PersonDocNumber { get; set; }
        public Guid PersonGUID { get; set; }
        public string MenuPermisos { get; set; }
        public Guid UserPlaceGuid { get; set; }
        public String UserPlaceName { get; set; }
        public string UserPlaceDescript { get; set; }
        public DateTime PersonModifiedDate { get; set; }
        public string ErrorMessage { set; get; }
    }
}