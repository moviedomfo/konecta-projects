//using Microsoft.AspNetCore.Builder;
//using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Web;
using System.Web.Http.Description;

public class AuthTokenOperation : IDocumentFilter
{
    public void Apply(SwaggerDocument swaggerDoc, SchemaRegistry schemaRegistry, IApiExplorer apiExplorer)
    {
        swaggerDoc.paths.Add("auth", new PathItem
        {
            post = new Operation
            {
                tags = new List<string> { "/api/oauth/auth" },
                consumes = new List<string>
                {
                    //"appliccation/x-www-form-urlencoded"
                    "application/json"
                },
                parameters = new List<Parameter>
                {
                    //new Parameter
                    //{
                    //    type = "string",
                    //    name = "grant_type",
                    //    required = true,
                    //    @in = "formData",
                    //    @default="password"
                    //},
                     new Parameter
                    {
                        type = "string",
                        name = "domain",
                        required = true,
                        @in = "formData",
                        @default="allus-ar"
                    },
                    new Parameter
                    {
                        type = "string",
                        name = "username",
                        required = true,
                        @in = "formData"
                    },
                    new Parameter
                    {
                        type = "string",
                        name = "password",
                        required = true,
                        @in = "formData"
                    }
                }

            }
        });
    }
}

public class AuthOperationFilter : IOperationFilter
{
    public void Apply(Operation operation, SchemaRegistry schemaRegistry, ApiDescription apiDescription)
    {
       if(operation.parameters == null)
        {
            operation.parameters = new List<Parameter>();
        }
        operation.parameters.Add(new Parameter
        {
            name = "Authorization",
            @in = "header",
            description = "access token",
            required= false,
            type= "string"

        });
    }
}

//namespace JwtSwaggerDemo.Infrastructure
//{
//    public static class SwaggerServiceExtensions
//    {
//        public static IServiceCollection AddSwaggerDocumentation(this IServiceCollection services)
//        {
//            services.AddSwaggerGen(c =>
//            {
//                c.SwaggerDoc("v1.0", new Info { Title = "Main API v1.0", Version = "v1.0" });

//                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
//                {
//                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
//                    Name = "Authorization",
//                    In = "header",
//                    Type = "apiKey"
//                });
//            });

//            return services;
//        }

//        public static IApplicationBuilder UseSwaggerDocumentation(this IApplicationBuilder app)
//        {
//            app.UseSwagger();
//            app.UseSwaggerUI(c =>
//            {
//                c.SwaggerEndpoint("/swagger/v1.0/swagger.json", "Versioned API v1.0");

//                c.DocExpansion("none");
//            });

//            return app;
//        }
//    }
//}