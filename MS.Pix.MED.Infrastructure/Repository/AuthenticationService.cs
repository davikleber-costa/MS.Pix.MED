using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Infrastructure.Repository;

public class AuthenticationService : IAuthenticationService
{
    private readonly IMemoryCache _cache;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthenticationService> _logger;
    private readonly JwtSecurityTokenHandler _tokenHandler;

    public AuthenticationService(
        IMemoryCache cache,
        IConfiguration configuration,
        ILogger<AuthenticationService> logger)
    {
        _cache = cache;
        _configuration = configuration;
        _logger = logger;
        _tokenHandler = new JwtSecurityTokenHandler();
    }

    public async Task<bool> ValidateTokenAsync(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
                return false;

            var principal = await GetClaimsPrincipalFromTokenAsync(token);
            return principal != null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao validar token");
            return false;
        }
    }

    public async Task<ClaimsPrincipal?> GetClaimsPrincipalFromTokenAsync(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
                return null;

            // Remove "Bearer " prefix se presente
            if (token.StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                token = token.Substring(7);

            var jwtToken = _tokenHandler.ReadJwtToken(token);
            
            if (IsTokenExpired(token))
                return null;

            var claims = jwtToken.Claims.ToList();
            var identity = new ClaimsIdentity(claims, "jwt");
            return new ClaimsPrincipal(identity);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao extrair claims do token");
            return null;
        }
    }

    public async Task<string?> GetUserIdFromTokenAsync(string token)
    {
        var principal = await GetClaimsPrincipalFromTokenAsync(token);
        return principal?.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? 
               principal?.FindFirst("sub")?.Value;
    }

    public async Task<IEnumerable<string>> GetUserRolesFromTokenAsync(string token)
    {
        var principal = await GetClaimsPrincipalFromTokenAsync(token);
        if (principal == null)
            return Enumerable.Empty<string>();

        return principal.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .Union(principal.FindAll("cognito:groups").Select(c => c.Value))
            .Distinct();
    }

    public bool IsTokenExpired(string token)
    {
        try
        {
            if (string.IsNullOrEmpty(token))
                return true;

            var jwtToken = _tokenHandler.ReadJwtToken(token);
            return jwtToken.ValidTo < DateTime.UtcNow;
        }
        catch
        {
            return true;
        }
    }
}