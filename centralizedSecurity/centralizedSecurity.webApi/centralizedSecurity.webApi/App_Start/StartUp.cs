﻿
using Microsoft.Owin;
using Owin;
using System.Web.Http;






//this class will be fired once our server starts, notice the “assembly” attribute which states which class to fire on start-up.

[assembly: OwinStartup(typeof(CentralizedSecurity.webApi.Startup))]
namespace CentralizedSecurity.webApi
{
    public partial class Startup
    {
        //public static OAuthAuthorizationServerOptions OAuthOptions { get; private set; }
        //public static GoogleOAuth2AuthenticationOptions googleAuthOptions { get; private set; }
        public static string PublicClientId { get; private set; }


        /// <summary>
        /// IAppBuilder” will be supplied by the host at run-time.
        /// </summary>
        /// <param name="app"></param>
        public void Configuration(IAppBuilder app)
        {
            //is used to configure API routes so we’ll pass this object to method “Register” in “WebApiConfig” class.
            HttpConfiguration httpConfig = new HttpConfiguration();
            ConfigureOAuthTokenGeneration(app);
			//flaw que indoca si se va a hjacer log de ShowPII Personally Identifiable Information (
            //Microsoft.IdentityModel.Logging.IdentityModelEventSource.ShowPII = true;
            //app.UseCors(CorsOptions.AllowAll);

            app.UseWebApi(httpConfig);


            //           GlobalConfiguration.Configuration.EnableSwagger(c =>
            //{
            //    var baseDirectory = AppDomain.CurrentDomain.BaseDirectory + @"\bin\";
            //    var commentsFileName = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
            //    var commentsFile = Path.Combine(baseDirectory, commentsFileName);

            //    c.SingleApiVersion("v1", "SomosTechies API")
            //      .Description("A sample API for testing")
            //      .TermsOfService("Some terms")
            //      .Contact(cc => cc
            //      .Name("Jesus Angulo")
            //      .Url("https://somostechies.com/contact")
            //      .Email("jesus.angulo@outlook.com"))
            //      .License(lc => lc
            //      .Name("Some License")
            //      .Url("https://somostechies.com/license"));

            //    c.IncludeXmlComments(commentsFile);

            //})
            //  .EnableSwaggerUi();

            string path = System.Web.HttpContext.Current.Server.MapPath(@"~\files\apiConfig.json");
            apiHelper.InitializeConfig(path);
        }



        // For more information on configuring authentication, please visit http://go.microsoft.com/fwlink/?LinkId=301864
        public void ConfigureOAuthTokenGeneration(IAppBuilder app)
        {
            // Configure the db context and user manager to use a single instance per request
            ////app.CreatePerOwinContext(ApplicationDbContext.Create);
            ////app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
            ////app.CreatePerOwinContext<ApplicationRoleManager>(ApplicationRoleManager.Create);
            ////app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);
            //app.CreatePerOwinContext<Fwk.BusinessFacades.SimpleFacade>(new Fwk.BusinessFacades.SimpleFacade);
            // Enable the application to use a cookie to store information for the signed in user
            // and to use a cookie to temporarily store information about a user logging in with a third party login provider
            //app.UseCookieAuthentication(new CookieAuthenticationOptions());

            //app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

            #region Authorization Server Configuration:
            // Configure the application for OAuth based flow
            PublicClientId = "self";



            //// Authorization Server middleware behavior 
            ////http://bitoftech.net/2015/02/16/implement-oauth-json-web-tokens-authentication-in-asp-net-web-api-and-identity-2/
            ////  Esta configuracion va en el Authorization Server: Token Issuer
            //OAuthOptions = new OAuthAuthorizationServerOptions()
            //{
            //    //For Dev enviroment only (on production should be AllowInsecureHttp = false)
            //    AllowInsecureHttp = true,
            //    TokenEndpointPath = new PathString("/oauth/token"),
            //    AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
            //    Provider = new CustomOAuthProvider(), //We’ve specified the implementation on how to validate the client and Resource owner user credentials in a custom class named “CustomOAuthProvider”.
            //    RefreshTokenProvider = new RefreshTokenProvider(),
            //    //The data format used to protect the information contained in the access token
            //    AccessTokenFormat = new CustomJwtFormat(),
            //    ApplicationCanDisplayErrors = true
            //};
            // OAuth 2.0 Bearer Access Token Generation
            //                app.UseOAuthBearerTokens(OAuthOptions);
            #endregion

        }


   
    }
}
