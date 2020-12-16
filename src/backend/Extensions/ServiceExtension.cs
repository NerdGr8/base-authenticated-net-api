
using JK.DAL;
using JK.DAL.Models;
using JK.Helpers;
using JK.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using OwaspHeaders.Core.Extensions;
using OwaspHeaders.Core.Models;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using Autofac;
using AutoMapper;
using JK.DAL.Mappings;
using Microsoft.OpenApi.Models;

namespace JK.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
        }

        public static void ConfigureSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c => {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "JK API",
                    Description = "JK API"
                });
            });
        }

        public static void ConfigureServices(this IServiceCollection services, IConfiguration Configuration)
        {
            services.AddScoped<JwtService>();
            services.AddScoped<RepositoryContext>();
            services.AddScoped<AuthService>();
            services.AddScoped<UserService>();
            services.AddScoped<EncryptionService>();

            //Applicatioon Data Layer Services
            services.AddScoped<UserRecordService>();

            //setup rabbitmq
            services.AddAutoMapper();
        }

        public static SecureHeadersMiddlewareConfiguration BuildDefaultConfiguration()
        {
            return SecureHeadersMiddlewareBuilder
                .CreateBuilder()
                .UseHsts()
                .UseXFrameOptions()
                .UseXSSProtection()
                .UseContentTypeOptions()
                .UseContentDefaultSecurityPolicy()
                .UsePermittedCrossDomainPolicies()
                .UseReferrerPolicy()
                .RemovePoweredByHeader()
                .Build();
        }

        public static IServiceCollection AddAutoMapper(this IServiceCollection services)
        {
            //Impliments AutoMapper
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new ModelToProfileMapper());
            });
            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);

            return services;
        }

    }
}