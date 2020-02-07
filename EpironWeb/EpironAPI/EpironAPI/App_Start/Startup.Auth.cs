
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Microsoft.Practices.EnterpriseLibrary.Data;
//using Microsoft.Owin.Security.DataHandler.Encoder;
//using Microsoft.Owin.Security.Jwt;
//using Microsoft.Owin.Security.OAuth;
using Owin;
using System;

using System.Web.Http;

//this class will be fired once our server starts, notice the “assembly” attribute which states which class to fire on start-up.
[assembly: OwinStartup(typeof(EpironAPI.App_Start.Startup))]
namespace EpironAPI.App_Start
{
	public class Startup
	{
     

        /// <summary>
        /// IAppBuilder” will be supplied by the host at run-time.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            
            //is used to configure API routes so we’ll pass this object to method “Register” in “WebApiConfig” class.
            HttpConfiguration httpConfig = new HttpConfiguration();
            //ConfigureOAuthTokenGeneration(app);


            app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);



        }

    }
}