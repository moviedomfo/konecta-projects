using System;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.ServiceModel.Channels;
using Fwk.Bases.Blocks.Fwk.BusinessFacades;
using System.Net;
using System.Linq;

using Fwk.BusinessFacades;
using Fwk.Bases;

namespace EpironAPI.Models
{
    public class BaseApiController : ApiController
    {
        private IBusinessFacade service;
        protected SimpleFacade simpleFacade;
        protected HostContext hostContext;
        BaseApiController(IBusinessFacade service)
        {
            this.service = service;
        }



        protected void CreateSimpleFacade()
        {
            if (simpleFacade == null)
            {
                simpleFacade = new Fwk.BusinessFacades.SimpleFacade();
            }
            if (hostContext == null)
            {
                string[] computer_name = null;
                hostContext = new HostContext();
                //OperationContext context = OperationContext.Current;
                //MessageProperties prop = context.IncomingMessageProperties;
                //RemoteEndpointMessageProperty endpoint = prop[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                //computer_name = Dns.GetHostEntry(endpoint.Address).HostName.Split(new Char[] { '.' });

                //hostContext.HostIp = endpoint.Address;

                hostContext.HostIp = GetClientIp();
                if (string.IsNullOrEmpty(hostContext.HostName))
                    computer_name = Dns.GetHostEntry("").HostName.Split(new Char[] { '.' });
                else
                    computer_name = Dns.GetHostEntry(hostContext.HostIp).HostName.Split(new Char[] { '.' });
                if (computer_name.Count() > 0)
                    hostContext.HostName = computer_name[0].ToString();
            }

        }

        public string GetIp()
        {
            return GetClientIp();
        }
        protected string GetClientIp(HttpRequestMessage request = null)
        {
            request = request ?? Request;

            if (request.Properties.ContainsKey("MS_HttpContext"))
            {
                return ((HttpContextWrapper)request.Properties["MS_HttpContext"]).Request.UserHostAddress;
            }
            else if (request.Properties.ContainsKey(RemoteEndpointMessageProperty.Name))
            {
                RemoteEndpointMessageProperty prop = (RemoteEndpointMessageProperty)request.Properties[RemoteEndpointMessageProperty.Name];
                return prop.Address;
            }
            else if (HttpContext.Current != null)
            {
                return HttpContext.Current.Request.UserHostAddress;
            }
            else
            {
                return null;
            }
        }
        public BaseApiController()
        {
        }



     




        protected override void Dispose(bool disposing)
        {


            base.Dispose(disposing);
        }
    }

   
}