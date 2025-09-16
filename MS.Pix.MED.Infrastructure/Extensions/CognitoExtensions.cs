using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace MS.Pix.MED.Infrastructure.Extensions;

public static class CognitoExtensions
{
    public static IServiceCollection AddCognitoAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var cognitoSettings = configuration.GetSection("CognitoSettings");
        var userPoolId = cognitoSettings["UserPoolId"];
        var clientId = cognitoSettings["ClientId"];
        var region = cognitoSettings["Region"];

        if (string.IsNullOrEmpty(userPoolId) || string.IsNullOrEmpty(clientId) || string.IsNullOrEmpty(region))
        {
            // Se as configurações do Cognito não estiverem presentes, pula a configuração
            return services;
        }

        var authority = $"https://cognito-idp.{region}.amazonaws.com/{userPoolId}";

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = authority,
                    ValidateAudience = true,
                    ValidAudience = clientId,
                    ValidateLifetime = true
                };
            });

        return services;
    }
}