using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using epironApi.webApi;
using epironApi.webApi.common;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace EpironApi_core_3._1
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
            services.AddControllers();
            services.AddCors();
            //services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

            #region load appSettings
            var serverSettings = new ServerSettings();
            Configuration.Bind("serverSettings", serverSettings);      //  <--- This
            services.AddSingleton(serverSettings);

            apiAppSettings.serverSettings = serverSettings;
            #endregion


            // configure DI for application services
            services.AddScoped<IEpironService, EpironService>();
            //services.AddScoped<ILDAPService, LDAPService>();

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
            var key = Encoding.ASCII.GetBytes(serverSettings.apiConfig.api_secretKey);


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
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "API bot comment",
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

                //c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                //{
                //    Description = "Please enter into field the word 'Bearer' following by space and JWT (using the Bearer scheme). Example: \"Authorization: Bearer {token}\"",
                //    Name = "Authorization",
                //    In = "header",
                //    Type = "apiKey",

                //});

                //c.AddSecurityDefinition("Bearer", new OAuth2Scheme()
                //{
                //    Flow = "password",
                //    Type = "oauth2",
                //    AuthorizationUrl = "https://localhost:44359/api/ldap/authTest",
                //    Scopes = new Dictionary<string, string> {
                //    { "demo_api", "Demo API - full access" }

                //}
                //});
                //c.OperationFilter<AuthorizeCheckOperationFilter>(); // Required to use access token
                //c.AddSecurityRequirement(security);
            });

            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseHttpsRedirection();

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

            //app.UseMvc();

            //app.Run( async(context))=>{

            //    await context.
            //});
            //if (env.IsDevelopment())
            //{
            //    app.UseDeveloperExceptionPage();
            //}

            //app.UseHttpsRedirection();

            //app.UseRouting();

            //app.UseAuthorization();

            //app.UseEndpoints(endpoints =>
            //{
            //    endpoints.MapControllers();
            //});
        }
    }
}
