using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CentralizedSecurity.webApi.Models
{
    public class LoginRequest
    {
        public string username { get; set; }
        public string password { get; set; }
        public string domain { get; set; }

        
    }


    public class userResetPasswordReq
    {
        public string WindowsUser { get; set; }
        public string newPassword { get; set; }
        public string DomainName { get; set; }

        public int dom_id { get; set; }
        public int Emp_Id { get; set; }

        /// <summary>
        /// Id del usuario logueado
        /// </summary>
        public int ResetUserId { get; set; }

        public string ResetUserName { get; set; }
        public bool ResetUserCAIS { get; set; }


        public string ticket { get; set; }

        /// <summary>
        /// con el nombre de la PC usada
        /// </summary>
        public string host { get; set; }


    }

    public class userUnlockReq
    {
        public string WindowsUser { get; set; }

        public string DomainName { get; set; }

        public int dom_id { get; set; }
        public int emp_id { get; set; }

        /// <summary>
        /// Nombre del usuario logueado
        /// </summary>
        public string ResetUserName { get; set; }
        /// <summary>
        /// Id del usuario logueado
        /// </summary>
        public int ResetUserId { get; set; }

        public bool ResetUserCAIS { get; set; }


        public string ticket { get; set; }

        /// <summary>
        /// con el nombre de la PC usada
        /// </summary>
        public string host { get; set; }


    }

    public class UserSetActivationReq
    {
        public string userName { get; set; }
        public bool disabled { get; set; }
        public string domain { get; set; }


    }
   

}