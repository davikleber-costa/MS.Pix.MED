using Microsoft.Extensions.DependencyInjection;

namespace MS.Pix.MED.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection UseTipoInfracao(this IServiceCollection services)
    {
        // Os handlers já são registrados automaticamente pelo MediatR
        // Esta extensão pode ser usada para configurações específicas do TipoInfracao
        return services;
    }

    public static IServiceCollection UseTransacao(this IServiceCollection services)
    {
        // Os handlers já são registrados automaticamente pelo MediatR
        // Esta extensão pode ser usada para configurações específicas da Transacao
        return services;
    }

    public static IServiceCollection UseRetornoJdpi(this IServiceCollection services)
    {
        // Os handlers já são registrados automaticamente pelo MediatR
        // Esta extensão pode ser usada para configurações específicas do RetornoJdpi
        return services;
    }
}