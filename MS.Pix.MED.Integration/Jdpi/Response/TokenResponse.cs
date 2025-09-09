using Newtonsoft.Json;

namespace MS.Pix.MED.Integration.Jdpi.Response;

public class TokenResponse
{
    [JsonProperty("expires_in")]
    public long ExpiresIn { get; set; }

    [JsonProperty("access_token")]
    public string AccessToken { get; set; } = string.Empty;

    [JsonProperty("token_type")]
    public string TokenType { get; set; } = string.Empty;

    public string Scope { get; set; } = string.Empty;
}