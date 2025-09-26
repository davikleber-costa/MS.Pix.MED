using Microsoft.Extensions.DependencyInjection;

namespace MS.Pix.MED.Application.Extensions;

public static class ApplicationExtensions
{
    public static IServiceCollection UseTipoInfracao(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection UseTransacao(this IServiceCollection services)
    {
        return services;
    }

    public static IServiceCollection UseRetornoJdpi(this IServiceCollection services)
    {
        return services;
    }
}
