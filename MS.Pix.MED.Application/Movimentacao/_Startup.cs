using Microsoft.Extensions.DependencyInjection;
using MS.Pix.MED.Application.Movimentacao.Commands;
using MS.Pix.MED.Application.Movimentacao.Queries;

namespace MS.Pix.MED.Application.Movimentacao;

public static class StartupMovimentacao
{
    public static IServiceCollection UseMovimentacao(this IServiceCollection services)
    {
        // Registrar handlers de Commands
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateMovimentacaoCommand>());
        
        // Registrar handlers de Queries
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ListMovimentacoesQuery>());
        
        return services;
    }
}