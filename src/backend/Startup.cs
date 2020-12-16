using System;
using System.Collections.Generic;
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
using Ocelot.Middleware;
using Ocelot.DependencyInjection;
using Ocelot.Provider.Polly;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Diagnostics;
using JK.Extensions;
using OwaspHeaders.Core.Extensions;
using JK.DAL.Models;
using JK.Helpers;
using JK.DAL;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;


namespace JK
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            //MvcOptions.EnableEndpointRouting = false;
            services.AddMvc(option => option.EnableEndpointRouting = false).SetCompatibilityVersion(CompatibilityVersion.Version_3_0);

            //assign values to settings class
            services.Configure<Settings>(options =>
            {
                options.ConnectionString = Configuration.GetSection("Postgres:ConnectionString").Value;
                options.SecretKey = Configuration.GetSection("Authentication:SecretKey").Value;
                options.Issuer = Configuration.GetSection("Authentication:Issuer").Value;
                options.ExpiresInMinutes = Convert.ToInt16(Configuration.GetSection("Authentication:ExpiresInMinutes").Value);
            });

            services.AddEntityFrameworkNpgsql().AddDbContext<RepositoryContext>(opt => {
                opt.UseNpgsql(Configuration["Postgres:ConnectionString"]);
                opt.UseLazyLoadingProxies().UseNpgsql(Configuration["Postgres:ConnectionString"]);
            });

            //add swagger 
            services.ConfigureSwagger();
            services.ConfigureCors();
            services.ConfigureServices(Configuration);

            //add jwt auth functionality
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Authentication:Issuer"],
                    ValidAudience = Configuration["Authentication:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Authentication:SecretKey"]))
                };
            });

            var container = new ContainerBuilder();
            container.Populate(services);

            return new AutofacServiceProvider(container.Build());
        }

        //This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
               // app.UseHsts();
            }

            //exception handling for 401 and 500
            app.UseExceptionHandler(appBuilder =>
            {
                appBuilder.Use(async (context, next) =>
                {
                    var error = context.Features[typeof(IExceptionHandlerFeature)] as IExceptionHandlerFeature;

                    //when authorization has failed, should retrun a json message to client
                    if (error != null && error.Error is SecurityTokenExpiredException)
                    {
                        context.Response.StatusCode = 401;
                        context.Response.ContentType = "application/json";

                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            status = 401,
                            message = "Its either token has expired or incorrect",
                            error = "Forbidden route"
                        }));
                    }
                    //when orther error, retrun a error message json to client
                    else if (error != null && error.Error != null)
                    {
                        context.Response.StatusCode = 500;
                        context.Response.ContentType = "application/json";
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(new
                        {
                            status = 500,
                            message = "Internal Server Error",
                            error = error.Error.Message
                        }));
                    }
                    //when no error, do next.
                    else await next();
                });
            });
            app.UseStaticFiles();
            //add swagger middleware and ui
            app.UseSwagger();
            app.UseSwaggerUI(c => {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "JK Api V1");
            });

            app.UseSecureHeadersMiddleware(ServiceExtensions.BuildDefaultConfiguration());

            app.UseAuthentication();

            //app.UseHttpsRedirection();
            
            app.UseMvc();

            app.UseCors();

        }

    }

}
