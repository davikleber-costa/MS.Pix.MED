using Microsoft.Extensions.DependencyInjection;
using MS.Pix.MED.Application.TipoInfracao.Commands;
using MS.Pix.MED.Application.TipoInfracao.Queries;

namespace MS.Pix.MED.Application.TipoInfracao;

public static class StartupTipoInfracao
{
    public static IServiceCollection UseTipoInfracao(this IServiceCollection services)
    {
        // Registrar handlers de Commands
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTipoInfracaoCommand>());
        
        // Registrar handlers de Queries
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ListTipoInfracoesQuery>());
        
        return services;
    }
}