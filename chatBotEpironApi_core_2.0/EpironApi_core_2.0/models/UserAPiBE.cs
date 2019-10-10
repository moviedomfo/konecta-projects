using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace epironApi.webApi.Models
{

    public class UserAPiBE
    {
        public int Emp_id { get; set; }
        public string ApeNom { get; set; }
       
        public string DNI { get; set; }
        public string Cuenta { get; set; }
        public string Cargo { get; set; }
        public string Subarea { get; set; }
        public string Dominio { get; set; }
        public int DomainId { get; set; }

        public int Legajo { get; set; }

        public bool CAIS { get; set; }
        public string WindowsUser { get; set; }
        public int? Aus_Id { get; set; }


        
    }
  



 

    public class DomainsBE
    {
        public string Domain { get; set; }
        public int DomainId { get; set; }
    }
}