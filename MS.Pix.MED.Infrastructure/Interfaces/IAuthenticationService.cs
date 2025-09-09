using System.Security.Claims;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IAuthenticationService
{
    Task<bool> ValidateTokenAsync(string token);
    Task<ClaimsPrincipal?> GetClaimsPrincipalFromTokenAsync(string token);
    Task<string?> GetUserIdFromTokenAsync(string token);
    Task<IEnumerable<string>> GetUserRolesFromTokenAsync(string token);
    bool IsTokenExpired(string token);
}