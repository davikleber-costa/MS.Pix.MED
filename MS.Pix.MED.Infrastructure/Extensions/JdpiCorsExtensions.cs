using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MS.Pix.MED.Infrastructure.Extensions;

public static class JdpiCorsExtensions
{
    public static IServiceCollection AddJdpiCors(this IServiceCollection services, IConfiguration configuration)
    {
        var jdpiSettings = configuration.GetSection("JdpiSettings");
        var allowedOrigins = jdpiSettings.GetSection("AllowedOrigins").Get<string[]>() ?? new[] { "*" };
        
        services.AddCors(options =>
        {
            options.AddPolicy("JdpiApiPolicy", policy =>
            {
                if (allowedOrigins.Contains("*"))
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader();
                }
                else
                {
                    policy.WithOrigins(allowedOrigins)
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .AllowCredentials();
                }
            });
        });

        return services;
    }
}