using Refit;

namespace MS.Pix.MED.Integration.Jdpi.Request;

public class TokenRequest
{
    [AliasAs("client_id")]
    public string ClientId { get; set; } = string.Empty;

    [AliasAs("client_secret")]
    public string ClientSecret { get; set; } = string.Empty;

    [AliasAs("grant_type")]
    public string GrantType { get; set; } = string.Empty;

    [AliasAs("scope")]
    public string Scope { get; set; } = string.Empty;
}