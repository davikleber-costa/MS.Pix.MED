using Microsoft.Extensions.DependencyInjection;
using MS.Pix.MED.Application.TipoTransaction.Commands;
using MS.Pix.MED.Application.TipoTransaction.Queries;

namespace MS.Pix.MED.Application.TipoTransaction;

public static class StartupTipoTransaction
{
    public static IServiceCollection UseTipoTransaction(this IServiceCollection services)
    {
        // Registrar handlers de Commands
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateTipoTransactionCommand>());
        
        // Registrar handlers de Queries
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ListTipoTransactionsQuery>());
        
        return services;
    }
}