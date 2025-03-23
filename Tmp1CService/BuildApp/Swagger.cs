using Microsoft.OpenApi.Models;

namespace Tmp1CService.BuildApp;

public static class Swagger
{
    public static void AddSecurity(WebApplicationBuilder builder)
    {
        builder.Services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition("ApiKeyScheme", new OpenApiSecurityScheme
            {
                Name = "X-API-KEY",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Description = "Введите API для авторизации"
            });

            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "ApiKeyScheme"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }

    public static void AddSwagger(WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
    }
}