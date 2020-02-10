using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace EpironBotTestAPI
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
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            #region Configuracion de middlewares para  que angular se levante junto a asp net core API
            //Estos middlewares se agreagan con la finalidad de acegurar  de que podamos configurar angular
            //para ser servidos desde esta carpeta wwwwroot
            //Con los meddlewares tenemos la posibilidad de sumergirnos en los canales de comunicacion de los 
            //requetets  (pipeline) y hacer algunos cambios o agregar alguna lógica

            //Aqui esperamos  el siguiente ; llamamos el delegado "next" Esto significa que cuando un req llega desde arriva
            //diremos que este le pegara primero  a este app.use middleware: (1.2) Tambien aqui esperaremos hasta q el response 
            // y si el response retorna 402 (file not found) y si 
            //Esto hacegura que cuando el browser haga un get con este response se retornara el index.html: El cual es manejado por angular
            app.Use(async (context, next) =>
            {
                await next(); //(1.2)
                if (context.Response.StatusCode == 404 && !Path.HasExtension(context.Request.Path.Value))
                {
                    context.Request.Path = "/index.html";
                    await next();
                }
            });
            app.UseDefaultFiles();

            //1 - este midleware habilita o le dice a nuestro asp net core que ie nuestro proyecto tambien 
            //usaremos archivos estaticos 
            app.UseStaticFiles();
            #endregion

            app.UseHttpsRedirection();
            app.UseMvc();
        }
    }
}
