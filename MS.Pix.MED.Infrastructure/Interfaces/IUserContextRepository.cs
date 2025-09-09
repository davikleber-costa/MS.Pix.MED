namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IUserContextRepository
{
    string? GetCurrentUserId();
    string? GetCurrentUserName();
    IEnumerable<string> GetCurrentUserRoles();
    bool IsUserInRole(string role);
    bool IsAuthenticated();
    string? GetClaimValue(string claimType);
}