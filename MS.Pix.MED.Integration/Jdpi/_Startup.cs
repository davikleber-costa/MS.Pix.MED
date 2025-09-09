using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MS.Pix.MED.Integration.Jdpi.Configuration;
using MS.Pix.MED.Integration.Jdpi.Services;

namespace MS.Pix.MED.Integration.Jdpi;

public static class StartupJdpi
{
    public static IServiceCollection UseJdpiApiIntegration(this IServiceCollection services, IConfiguration configuration)
    {
        // Configurar opções para API JDPI externa
        services.Configure<JdpiConfiguration>(configuration.GetSection(JdpiConfiguration.SectionName));

        // Registrar HttpClient para comunicação direta com API JDPI
        services.AddHttpClient<IJdpiIntegrationService, JdpiIntegrationService>((serviceProvider, client) =>
        {
            var jdpiConfig = configuration.GetSection(JdpiConfiguration.SectionName).Get<JdpiConfiguration>();
            if (jdpiConfig != null)
            {
                client.BaseAddress = new Uri(jdpiConfig.BaseUrl);
                client.Timeout = TimeSpan.FromSeconds(jdpiConfig.TimeoutSeconds);
                
                // Headers padrão para API externa
                client.DefaultRequestHeaders.Add("User-Agent", "MS.Pix.MED-ApiClient/1.0");
                client.DefaultRequestHeaders.Add("Accept", "application/json");
                
                // Headers customizados se configurados
                if (jdpiConfig.DefaultHeaders != null)
                {
                    foreach (var header in jdpiConfig.DefaultHeaders)
                    {
                        client.DefaultRequestHeaders.Add(header.Key, header.Value);
                    }
                }
            }
        });

        // Registrar serviço de integração
        services.AddScoped<IJdpiIntegrationService, JdpiIntegrationService>();
        
        return services;
    }

}

// Extensão para configuração de CORS específica para API JDPI
public static class JdpiCorsExtensions
{
    public static IServiceCollection AddJdpiCors(this IServiceCollection services)
    {
        services.AddCors(options =>
        {
            options.AddPolicy("JdpiApiPolicy", policy =>
            {
                policy.AllowAnyOrigin()
                      .AllowAnyMethod()
                      .AllowAnyHeader();
            });
        });
        
        return services;
    }
}