using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EpironAPI.classes
{
    public static class Encriptador
    {
        public static string encriptar(string param)
        {
            return Fwk.Security.Cryptography.SymetricCypherFactory.Cypher().Encrypt(param);
        }

        public static string desencriptar(string param)
        {
            return Fwk.Security.Cryptography.SymetricCypherFactory.Cypher().Dencrypt(param);
        }
    }
}