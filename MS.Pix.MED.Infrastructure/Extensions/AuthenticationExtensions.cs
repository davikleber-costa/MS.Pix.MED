using Microsoft.Extensions.DependencyInjection;
using MS.Pix.MED.Infrastructure.Interfaces;
using MS.Pix.MED.Infrastructure.Repository;

namespace MS.Pix.MED.Infrastructure.Extensions;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddAuthenticationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IUserContextRepository, UserContextRepository>();
        
        return services;
    }
}