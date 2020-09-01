using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(EpironAPIDispatcher.App_Start.Startup))]
namespace EpironAPIDispatcher.App_Start
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


            //app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);



        }

    }
}