using Microsoft.Extensions.DependencyInjection;
using MS.Pix.MED.Application.JdpiLog.Commands;
using MS.Pix.MED.Application.JdpiLog.Queries;

namespace MS.Pix.MED.Application.JdpiLog;

public static class StartupJdpiLog
{
    public static IServiceCollection UseJdpiLog(this IServiceCollection services)
    {
        // Registrar handlers de Commands
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateJdpiLogCommand>());
        
        // Registrar handlers de Queries
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<ListJdpiLogsQuery>());
        
        return services;
    }
}