using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralizedSecurity.webApi.Models
{

    public class EmpleadoReseteoBE
    {
        public bool isRessetUser { get; set; }

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
    public class EmpleadoBE
    {
        

        public int Emp_id { get; set; }
        public string ApeNom { get; set;}
        //public string Apellido { get; set; }
        //public string Nombre { get; set; }
        public string DNI { get; set; }
        public string Cuenta { get; set; }
        public string Cargo { get; set; }
        public string Subarea { get; set; }
        //public string Dominio { get; set; }
     
        
        public int Legajo { get; set; }

        //public bool CAIS { get; set; }
        //public string WindowsUser { get; set; }
        public int? Aus_Id { get; set; }


        public List<WindosUserBE> WindosUserList{ get; set; }
        public string Telefono { get;  set; }
        public string Direccion { get;  set; }
        public string CiudadNatal { get;  set; }
        public string FechaNacimiento { get;  set; }

        /// <summary>
        /// Interno o Externo
        /// I o E
        /// </summary>
        public string Tipo { get;  set; }
        public string Email { get;  set; }
    }


    public class WindosUserBE
    {
        public int Emp_id { get; set; }
        public string Dominio { get; set; }
        public string WindowsUser { get; set; }
        public int dom_id { get; set; }

    }


    public class DomainsBE
    {
        public string Domain { get; set; }
        public int DomainId { get; set; }
    }
}