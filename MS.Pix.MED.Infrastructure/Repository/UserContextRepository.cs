using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Infrastructure.Repository;

public class UserContextRepository : IUserContextRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserContextRepository(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    private ClaimsPrincipal? CurrentUser => _httpContextAccessor.HttpContext?.User;

    public string? GetCurrentUserId()
    {
        return CurrentUser?.FindFirst(ClaimTypes.NameIdentifier)?.Value ??
               CurrentUser?.FindFirst("sub")?.Value;
    }

    public string? GetCurrentUserName()
    {
        return CurrentUser?.FindFirst(ClaimTypes.Name)?.Value ??
               CurrentUser?.FindFirst("cognito:username")?.Value ??
               CurrentUser?.FindFirst("username")?.Value;
    }

    public IEnumerable<string> GetCurrentUserRoles()
    {
        if (CurrentUser == null)
            return Enumerable.Empty<string>();

        return CurrentUser.FindAll(ClaimTypes.Role)
            .Select(c => c.Value)
            .Union(CurrentUser.FindAll("cognito:groups").Select(c => c.Value))
            .Distinct();
    }

    public bool IsUserInRole(string role)
    {
        return GetCurrentUserRoles().Contains(role, StringComparer.OrdinalIgnoreCase);
    }

    public bool IsAuthenticated()
    {
        return CurrentUser?.Identity?.IsAuthenticated ?? false;
    }

    public string? GetClaimValue(string claimType)
    {
        return CurrentUser?.FindFirst(claimType)?.Value;
    }
}