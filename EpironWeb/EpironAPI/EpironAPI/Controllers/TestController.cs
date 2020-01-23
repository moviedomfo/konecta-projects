using EpironAPI.BE;
using EpironAPI.classes;
using EpironAPI.Model;
using EpironAPI.Models;
using fwk.security.webApi;
using Fwk.Security.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Security;

namespace EpironAPI.Controllers
{
    [RoutePrefix("api/test")]   
    public class Testontroller : BaseApiController
    {
        [HttpPost]
        [AllowAnonymous]
        [Route("authenticate")]
        public async Task<HttpResponseMessage> authenticate(OauthRequets context)
        {
            StringBuilder errorMessages = new StringBuilder();
            SecurityClient securityClient = null;


            #region validations
            if (context.client_id == null)
            {
                //Remove the comments from the below line context.SetError, and invalidate context 
                //if you want to force sending clientId/secrects once obtain access tokens. 

                errorMessages.Append("invalid_client: IdDebe enviar el client_id");
            }


            try
            {
                securityClient = SecurityManager.ClientFind(context.client_id, context.securityProviderName);
            }
            catch (Exception ex)
            {

                errorMessages.Append("invalid_clientId:" + ex.Message);

            }
            if (securityClient == null && errorMessages.Length == 0)
            {
                errorMessages.Append("invalid_clientId:" + string.Format("Client '{0}' is not registered in the system.", context.client_id));
            }
            if (errorMessages.Length != 0)
            {
                return apiHelper.fromErrorString(errorMessages.ToString(), System.Net.HttpStatusCode.InternalServerError);
            }
            if (securityClient.ApplicationType == (int)ApplicationTypes.NativeConfidential)
            {
                if (string.IsNullOrWhiteSpace(context.client_secret))
                {
                    errorMessages.Append("invalid_clientId:  Client secret should be sent");
                }
                else
                {
                    if (securityClient.Secret != helper.GetHash(context.client_secret))
                    {

                        errorMessages.Append("invalid_clientId:  Client secret is invalid");
                    }
                }
            }

            if (!securityClient.Active)
            {
                errorMessages.Append("invalid_clientId:  Client is inactive.");
            }
            if (errorMessages.Length != 0)
                return apiHelper.fromErrorString(errorMessages.ToString(), System.Net.HttpStatusCode.InternalServerError);
            #endregion

            #region auth
            //string securityProviderName = context.OwinContext.Get<string>("as:securityProviderName");

            LoginResult res = SecurityManager.User_Authenticate(context.userName, context.password, context.securityProviderName);
            //context.SetError("invalid_grant", "LockedOut");

            SignInStatus s = (SignInStatus)Enum.Parse(typeof(SignInStatus), res.Status);
            switch (s)
            {
                case SignInStatus.Success:
                    {
                        res.Status = "Success";
                        break;
                    }
                default://HttpStatusCode.Forbidden = 403
                    {
                        //context.SetError(res.Status, res.Message);
                        errorMessages.Append(res.Message);
                        break;
                    }


            }
            #endregion

            #region old code   generamos los claims para este usuario
            // ClaimsIdentity oAuthIdentity = SecurityManager.GenerateClaimsIdentity(res.User);
            //oAuthIdentity.AddClaim(new Claim("securityProviderName", context.securityProviderName));

            //List<string> roles = null;
            //if (res.User.SecurityRoles.Count != 0)
            //{
            //    roles = new List<String>();
            //    res.User.SecurityRoles.ToList(). ForEach(r =>
            //    {
            //        roles.Add(r.Name);
            //    });
            //}




            // var props = new AuthenticationProperties();
            /////Agrega esta propiedad para que pueda ser leida por el CustomJwtFormat provider en el consumidor 
            //props.Dictionary.Add("securityProviderName", context.securityProviderName);
            //props.Dictionary.Add("client_id", context.client_id);
            //props.Dictionary.Add("userName", context.username);
            //props.Dictionary.Add("userId", res.User.Id.ToString());
            //props.Dictionary.Add("lastLogInDate", res.User.LastLogInDate.ToString());
            //if (!string.IsNullOrEmpty(res.User.Email))
            //    props.Dictionary.Add("email", res.User.Email);

            //if (roles != null)
            //    props.Dictionary.Add("roles", Fwk.HelperFunctions.FormatFunctions.GetStringBuilderWhitSeparator(roles, ',').ToString());
            //props.Dictionary.Add("Status", res.Status);



            ////Construccion de ticket
            //var ticket = new AuthenticationTicket(oAuthIdentity, props);

            //CustomJwtFormat jwtFormat = new CustomJwtFormat();
            //ticket.Properties.IssuedUtc = System.DateTime.Now;
            //ticket.Properties.ExpiresUtc = System.DateTime.Now.AddMinutes(1000);
            //var jwt = jwtFormat.Protect(ticket);
            #endregion
            if (errorMessages.Length != 0)
                return apiHelper.fromErrorString(errorMessages.ToString(), System.Net.HttpStatusCode.InternalServerError);
            var rolesArray = res.User.GetRolesArray();
            var jwt = TokenGenerator.GenerateTokenJwt(res.User.Id, context.userName, res.User.Email, rolesArray, context.securityProviderName);


            return apiHelper.fromObject<String>(jwt);
            //return Ok(jwt);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("createApplication")]
        public async Task<IHttpActionResult> CreateApplication(SecurityClient client)
        {

            client.Secret = helper.GetHash(client.Secret);

            SecurityManager.ClientCreate(client, "");

            return Ok();
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("hashClientSecret")]
        public string hashClientSecret(string clientSecret)
        {
            return helper.GetHash(clientSecret);
        }


        [Route("execute")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Execute(ExecuteReq req)
        {
            try
            {
                var o = new { ExecuteMessage = "El servicio 'Execute' respondio corerctamente " };
                return apiHelper.fromObject(o);
            }
            catch (Exception ex)
            {
                return apiHelper.fromEx(ex);
            }
        }

        [HttpGet]
        [AllowAnonymous]
        [Route("check")]
        public HttpResponseMessage check()
        {

            var o = new { ExecuteMessage = "El servicio 'check' respondio corerctamente " };
            return apiHelper.fromObject(o);
        }

        [AllowAnonymous]
        [Route("auth")]
        [System.Web.Http.HttpPost]
        public HttpResponseMessage Auth(ValidarAplicacionReq req)
        {
            return apiHelper.fromErrorString("ValidarAplicacion no implementado ", HttpStatusCode.MethodNotAllowed);
        }

     

       

    }
}
