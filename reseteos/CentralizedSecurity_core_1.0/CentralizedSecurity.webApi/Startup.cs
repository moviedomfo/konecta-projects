using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using CentralizedSecurity.webApi.common;
using CentralizedSecurity.webApi.helpers;
using IdentityServer4.AccessTokenValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace CentralizedSecurity.webApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }




        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);


            //services.a(  // enable Configuration Services
            #region load appSettings
            var appSettings = new serverSettings();
            Configuration.Bind("serverSettings", appSettings);      //  <--- This
            services.AddSingleton(appSettings);
            //https://weblog.west-wind.com/posts/2017/dec/12/easy-configuration-binding-in-aspnet-core-revisited
            apiAppSettings.serverSettings = appSettings;
            #endregion


            // configure DI for application services
            services.AddScoped<IMeucciService, MeucciService>();
            services.AddScoped<ILDAPService, LDAPService>();

            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            // .AddIdentityServerAuthentication(options =>
            // {
            //        // auth server base endpoint (will use to search for disco doc)
            //        options.Authority = "https://localhost:44359/";
            //        options.ApiName = "demo_api"; // required audience of access tokens
            //        options.RequireHttpsMetadata = false; // dev only!
            //    });

            #region configure jwt authentication
            //  var appSettings = appSettingsSection.Get<serverSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.apiConfig.api_secretKey);


            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    RequireExpirationTime = false,
                    ValidateLifetime = true,
                    ValidateAudience = false
                };
            });

            #endregion

            IdentityModelEventSource.ShowPII = true;
            //services.AddAuthentication(IdentityServerAuthenticationDefaults.AuthenticationScheme)
            // .AddIdentityServerAuthentication(options =>
            // {
            //     options.Authority = "http://localhost:5000"; // auth server base endpoint (will use to search for disco doc)
            //     options.ApiName = "demo_api"; // required audience of access tokens
            //     options.RequireHttpsMetadata = false; // dev only!
            // });

            #region servicios  de swagger

            // Register the Swagger generator, defining 1 or more Swagger documents
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Title = "API doc de Reseteos",
                    Version = "v1"
                });

                // Set the comments path for the Swagger JSON and UI.
                //For Linux or non-Windows operating systems, file names and paths can be case-sensitive. 
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "Please enter into field the word 'Bearer' following by space and JWT (using the Bearer scheme). Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",

                });

                //c.AddSecurityDefinition("Bearer", new OAuth2Scheme()
                //{
                //    Flow = "password",
                //    Type = "oauth2",
                //    AuthorizationUrl = "https://localhost:44359/api/ldap/authTest",
                //    Scopes = new Dictionary<string, string> {
                //    { "demo_api", "Demo API - full access" }

                //}
                //});
                c.OperationFilter<AuthorizeCheckOperationFilter>(); // Required to use access token
                c.AddSecurityRequirement(security);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}
            //else
            //{
            //    app.UseHsts();
            //}

            //app.UseHttpsRedirection();
            // global cors policy
            app.UseCors(x => x
                .AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());



            app.UseAuthentication();


            #region servicios meidleware de swagger
            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.),
            // specifying the Swagger JSON endpoint.
            ///https://localhost:44359/swagger/v1/swagger.json
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Doc. API Reseteos");
                c.RoutePrefix = String.Empty; //To serve the Swagger UI at the app's root (http://localhost:<port>/) -->   https://localhost:44359/swagger
                                              // c.DocExpansion("none");
            });
            #endregion

            app.UseMvc();
        }
    }

    public class AuthorizeCheckOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            //var hasAuthorize = context.ApiDescription. (true).OfType<AuthorizeAttribute>().Any();
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is AuthorizeFilter);
            var allowAnonymous = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is IAllowAnonymousFilter);

            if (isAuthorized && !allowAnonymous)
            {
                if (operation.Parameters == null)
                {
                    operation.Parameters = new List<IParameter>();
                }

                var tokenParameter = new NonBodyParameter
                {
                    Type = "string",
                    In = "header",
                    Name = "Authorization",
                    Description = "token",
                    Default = "Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOiJsb2NhbGhvc3QiLCJhdXRob3JpdHkiOiJhdXRob3JpdHkiLCJmb28iOiJiYXIiLCJpc3MiOiJ3ZSB0aGUgbWFueSJ9.bm77vM2yQXc93vnc44Rqv_Rkm5OszFa9daM37db6EBg",
                    Required = true
                };

                operation.Parameters.Add(tokenParameter);
            }
        }
    }
}
