using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using MS.Pix.MED.Integration.Jdpi.Request;
using MS.Pix.MED.Integration.Jdpi.Response;
using System.Net.Http.Headers;

namespace MS.Pix.MED.Integration.Jdpi;

public class AuthenticatedHttpClientHandler : DelegatingHandler
{
    private readonly IAuthJdpi _authApi;
    private readonly IMemoryCache _cache;
    private readonly JdpiOptions _jdpiOptions;

    public AuthenticatedHttpClientHandler(
        IAuthJdpi authApi, 
        IMemoryCache cache, 
        IOptions<JdpiOptions> jdpiOptions)
    {
        _authApi = authApi;
        _cache = cache;
        _jdpiOptions = jdpiOptions.Value;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, 
        CancellationToken cancellationToken)
    {
        var token = await GetTokenAsync();

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken);
    }

    private async Task<string> GetTokenAsync()
    {
        const string tokenKeyValue = "jdpi_access_token";

        var tokenResponse = _cache.Get<TokenResponse>(tokenKeyValue);

        if (tokenResponse is not null && !IsTokenExpiringSoon(tokenResponse))
            return tokenResponse.AccessToken;

        var token = await GenerateTokenAsync();

        SaveTokenMemoryCache(tokenKeyValue, token);

        return token.AccessToken;
    }

    private bool IsTokenExpiringSoon(TokenResponse tokenResponse)
    {
        var expirationTime = DateTimeOffset.FromUnixTimeSeconds(tokenResponse.ExpiresIn);
        var remainingTime = expirationTime - DateTimeOffset.UtcNow;

        return remainingTime <= TimeSpan.FromSeconds(_jdpiOptions.TokenRefreshBufferTimeInSeconds);
    }

    private Task<TokenResponse> GenerateTokenAsync()
    {
        var tokenRequest = new TokenRequest
        {
            ClientId = _jdpiOptions.ClientId,
            ClientSecret = _jdpiOptions.ClientSecret,
            GrantType = _jdpiOptions.GrantType,
            Scope = _jdpiOptions.Scope,
        };

        return _authApi.GetTokenAsync(tokenRequest);
    }

    private void SaveTokenMemoryCache(string key, TokenResponse token)
    {
        var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetAbsoluteExpiration(DateTimeOffset.FromUnixTimeSeconds(token.ExpiresIn));

        _cache.Set(key, token, cacheEntryOptions);
    }
}

public class JdpiOptions
{
    public string ClientId { get; set; } = string.Empty;

    public string ClientSecret { get; set; } = string.Empty;

    public string GrantType { get; set; } = "client_credentials";

    public string Scope { get; set; } = string.Empty;

    public string BaseUrl { get; set; } = string.Empty;

    public string AuthBaseUrl { get; set; } = string.Empty;

    public int TokenRefreshBufferTimeInSeconds { get; set; } = 300;
}