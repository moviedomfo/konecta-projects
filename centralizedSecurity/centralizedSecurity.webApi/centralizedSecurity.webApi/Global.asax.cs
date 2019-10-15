using Swashbuckle.Application;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace centralizedSecurity.webApi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            
            GlobalConfiguration.Configure(WebApiConfig.Register);

            //GlobalConfiguration.Configuration
            //    .EnableSwagger(c => c.SingleApiVersion("v1", "SomosTechies API"))
            //    .EnableSwaggerUi();
         //   SwaggerConfig.Register();
        }
    }
}
