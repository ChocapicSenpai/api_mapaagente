    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using Microsoft.Extensions.Hosting;
    using Microsoft.Extensions.Logging;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Newtonsoft.Json.Serialization;
    using Microsoft.Extensions.FileProviders;
    using System.IO;
    using Microsoft.OpenApi.Models;
    using Swashbuckle.AspNetCore.Swagger;
    using WebApplication2.Modelos;
    using WebApplication2.Context;
    using Microsoft.AspNetCore.Authentication.JwtBearer;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WebApplication2

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
            ////services.Configure<RecaptchaSettings>(Configuration.GetSection("RecaptchaSettings"));


            // Configurar autenticación JWT
            var jwtSettings = Configuration.GetSection("JwtSettings");
            var secretKey = jwtSettings["SecretKey"];
            var key = Encoding.ASCII.GetBytes(secretKey);

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
               .AddJwtBearer(options =>
               {
                   options.TokenValidationParameters = new TokenValidationParameters
                   {
                       ValidateIssuer = false,
                       ValidateAudience = false,
                       ValidateLifetime = true,
                       ValidateIssuerSigningKey = true,
                       IssuerSigningKey = new SymmetricSecurityKey(key)
                   };
               });
            // Reemplaza con tu propia clave secreta

            //services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            //.AddJwtBearer(options =>
            //{
            //    options.TokenValidationParameters = new TokenValidationParameters
            //    {
            //        ValidateIssuer = false,
            //        ValidateAudience = false,
            //        ValidateLifetime = true,
            //        ValidateIssuerSigningKey = true,
            //        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["JwtSettings:SecretKey"]))
            //    };
            //});
                
            //Enable CORS
            services.AddCors(c =>
                {
                    c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                });


                //JSON Serializer
                services.AddControllersWithViews().AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore)
                    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver
                    = new DefaultContractResolver());

                services.AddControllers();
            services.AddMvc();
                // Register DbContext
                services.AddDbContext<AuthDbContext>(options =>
                {
                    options.UseSqlServer(Configuration.GetConnectionString("AgenteAppCon"));

                    // Desactiva la validación del certificado SSL
                    // Desactiva la validación del certificado SSL
                    options.ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.MultipleCollectionIncludeWarning));
                    options.ConfigureWarnings(warnings => warnings.Ignore(2051));

                });


                        

                //SwaggerConfiguration
                services.AddSwaggerGen(c =>
                {
                    c.SwaggerDoc("v1", new OpenApiInfo { Title = "API", Version = "v1" });
                });


            }


            // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
            public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
            {
                //Enable CORS
                app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }
                //app.UseHttpsRedirection();

                app.UseRouting();

                app.UseAuthentication(); // Agregar middleware de autenticación

                app.UseAuthorization();


                app.UseEndpoints(endpoints =>
                {
                    endpoints.MapControllers();
                });

                // Enable Swagger
                app.UseSwagger();

                // Enable Swagger UI
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
                });

            }
        }
    }