using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Integration.Jdpi.Configuration;
using MS.Pix.MED.Integration.Jdpi.Request;
using MS.Pix.MED.Integration.Jdpi.Response;
using Newtonsoft.Json;
using System.Text;
using System.Collections.Concurrent;

namespace MS.Pix.MED.Integration.Jdpi.Services;

public class JdpiIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly JdpiConfiguration _configuration;
    private readonly ILogger<JdpiIntegrationService> _logger;
    private readonly ConcurrentDictionary<string, (string Token, DateTime Expiry)> _tokenCache;
    private string? _accessToken;
    private DateTime _tokenExpiry;

    public JdpiIntegrationService(
        HttpClient httpClient,
        IOptions<JdpiConfiguration> configuration,
        ILogger<JdpiIntegrationService> logger)
    {
        _httpClient = httpClient;
        _configuration = configuration.Value;
        _logger = logger;
        _tokenCache = new ConcurrentDictionary<string, (string, DateTime)>();
        
        _httpClient.BaseAddress = new Uri(_configuration.BaseUrl);
        _httpClient.Timeout = TimeSpan.FromSeconds(_configuration.TimeoutSeconds);
        
        if (_configuration.DefaultHeaders != null)
        {
            foreach (var header in _configuration.DefaultHeaders)
            {
                _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
            }
        }
    }

    public async Task<bool> AuthenticateAsync()
    {
        try
        {
            var cacheKey = $"{_configuration.ClientId}:{_configuration.Scope}";
            
            // Verifica cache de token se habilitado
            if (_configuration.EnableTokenCache && _tokenCache.TryGetValue(cacheKey, out var cachedToken))
            {
                if (DateTime.UtcNow < cachedToken.Expiry)
                {
                    _accessToken = cachedToken.Token;
                    _tokenExpiry = cachedToken.Expiry;
                    return true;
                }
                _tokenCache.TryRemove(cacheKey, out _);
            }

            // Verifica token atual
            if (!string.IsNullOrEmpty(_accessToken) && DateTime.UtcNow < _tokenExpiry)
            {
                return true;
            }

            _logger.LogInformation("Iniciando autenticação com API JDPI externa");

            var tokenRequest = new TokenRequest
            {
                ClientId = _configuration.ClientId,
                ClientSecret = _configuration.ClientSecret,
                GrantType = "client_credentials",
                Scope = _configuration.Scope ?? string.Empty
            };

            var formContent = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("client_id", tokenRequest.ClientId),
                new KeyValuePair<string, string>("client_secret", tokenRequest.ClientSecret),
                new KeyValuePair<string, string>("grant_type", tokenRequest.GrantType),
                new KeyValuePair<string, string>("scope", tokenRequest.Scope)
            });

            using var authClient = new HttpClient { BaseAddress = new Uri(_configuration.AuthUrl) };
            var response = await authClient.PostAsync("/connect/token", formContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                if (tokenResponse != null)
                {
                    _accessToken = tokenResponse.AccessToken;
                    _tokenExpiry = DateTime.UtcNow.AddSeconds(tokenResponse.ExpiresIn - 60);
                    
                    // Armazena no cache se habilitado
                    if (_configuration.EnableTokenCache)
                    {
                        var cacheExpiry = DateTime.UtcNow.AddMinutes(_configuration.TokenCacheMinutes);
                        _tokenCache.TryAdd(cacheKey, (_accessToken, cacheExpiry));
                    }
                    
                    _logger.LogInformation("Autenticação com API JDPI externa realizada com sucesso");
                    return true;
                }
            }

            _logger.LogError("Falha na autenticação com API JDPI: {StatusCode} - {Content}", response.StatusCode, responseContent);
            return false;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro durante autenticação com API JDPI externa");
            return false;
        }
    }

    public async Task<RetornoJdpi> SendToJdpiAsync(string endpoint, object requestData, long transacaoId, CancellationToken cancellationToken = default)
    {
        var retornoJdpi = new RetornoJdpi
        {
            TransacaoId = transacaoId,
            RequisicaoJdpi = JsonConvert.SerializeObject(requestData),
            DataCriacao = DateTime.UtcNow,
            HoraCriacao = TimeOnly.FromDateTime(DateTime.UtcNow).ToTimeSpan()
        };

        try
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            _logger.LogInformation("Enviando dados para API JDPI externa - Endpoint: {Endpoint}", endpoint);

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            var json = JsonConvert.SerializeObject(requestData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"{_configuration.BaseUrl}/{endpoint}", content, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

            retornoJdpi.RespostaJdpi = responseContent;

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Dados enviados com sucesso para API JDPI externa");
            }
            else
            {
                _logger.LogWarning("Resposta de erro da API JDPI externa: {StatusCode} - {Content}", response.StatusCode, responseContent);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar dados para API JDPI externa");
            retornoJdpi.RespostaJdpi = $"Erro: {ex.Message}";
        }

        return retornoJdpi;
    }

    public async Task<TResponse> QueryJdpiAsync<TResponse>(string endpoint, object? queryParameters = null, CancellationToken cancellationToken = default)
    {
        try
        {
            await EnsureAuthenticatedAsync(cancellationToken);

            _logger.LogInformation("Consultando dados da API JDPI externa - Endpoint: {Endpoint}", endpoint);

            _httpClient.DefaultRequestHeaders.Authorization = 
                new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", _accessToken);

            var queryString = BuildQueryString(queryParameters);
            var url = $"{_configuration.BaseUrl}/{endpoint}{queryString}";

            var response = await _httpClient.GetAsync(url, cancellationToken);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
            return JsonConvert.DeserializeObject<TResponse>(responseContent) ?? default(TResponse)!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar dados da API JDPI externa");
            throw;
        }
    }

    public async Task<RetornoJdpi> ProcessRefundAsync(object refundData, long transacaoId, CancellationToken cancellationToken = default)
    {
        return await SendToJdpiAsync("devolucao", refundData, transacaoId, cancellationToken);
    }

    public async Task<TResponse> ListRefundsAsync<TResponse>(object? filters = null, CancellationToken cancellationToken = default)
    {
        return await QueryJdpiAsync<TResponse>("devolucao/listar", filters, cancellationToken);
    }

    private async Task EnsureAuthenticatedAsync(CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrEmpty(_accessToken) || DateTime.UtcNow >= _tokenExpiry)
        {
            await AuthenticateAsync();
        }
    }

    private static string BuildQueryString(object? parameters)
    {
        if (parameters == null) return string.Empty;

        var properties = parameters.GetType().GetProperties();
        var queryParams = properties
            .Where(p => p.GetValue(parameters) != null)
            .Select(p => $"{p.Name}={Uri.EscapeDataString(p.GetValue(parameters)?.ToString() ?? "")}");

        var queryString = string.Join("&", queryParams);
        return string.IsNullOrEmpty(queryString) ? "" : $"?{queryString}";
    }
}