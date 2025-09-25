using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using MS.Pix.MED.Infrastructure.Data;
using MS.Pix.MED.Infrastructure.Interfaces;
using MS.Pix.MED.Infrastructure.Repository;
using MS.Pix.MED.Infrastructure.Extensions;
using MS.Pix.MED.Domain.Interfaces;

namespace MS.Pix.MED.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // DbContext registration
        services.AddDbContext<MedDbContext>(options =>
            options.UseNpgsql(connectionString));

        // MySQL Context registration
        services.AddScoped<IMySqlContext, MySqlContext>();

        // Repository registrations
        services.AddScoped<ITipoInfracaoRepository, TipoInfracaoRepository>();
        services.AddScoped<ITransacaoRepository, TransacaoRepository>();
        services.AddScoped<IRetornoJdpiRepository, RetornoJdpiRepository>();
        services.AddScoped<IPixTransacaoRepository, PixTransacaoRepository>();

        // Authentication services
        services.AddAuthenticationServices();

        return services;
    }
}