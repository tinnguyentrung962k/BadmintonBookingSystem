﻿using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;

namespace BadmintonBookingSystem.Configurations
{
    public static class SwaggerService
    {
        public static void AddSwaggerService(this IServiceCollection services)
        {
            services.AddSwaggerGen(
                c =>
                {
                    c.SwaggerDoc("v1",
                        new OpenApiInfo { Title = "Management Api", Version = "v1" });
                    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                    {
                        Description = "Standard Authorization header using the Bearer scheme. Example: \"bearer {token}\"",
                        In = ParameterLocation.Header,
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey
                    });
                    c.OperationFilter<SecurityRequirementsOperationFilter>();
                }
            );
        }
    }
}
