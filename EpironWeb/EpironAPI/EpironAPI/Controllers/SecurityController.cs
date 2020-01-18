using EpironAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EpironAPI.Controllers
{
    public class SecurityController : ApiController
    {
        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        
        public void ValidarAplicacion(ValidarAplicacionReq req)
        {
        }

        
        public void UserAutenticacion(UserAutenticacionReq req)
        {

            Guid guidSession =  Guid.NewGuid();

        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
