using System.Text;
using System.Text.Json;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MS.Pix.MED.Test.Temp;

/// <summary>
/// Classe de teste para verificar a integração com as rotas de infração do credisis-jdpi-core
/// Testa especificamente as rotas: /api/infracoes/relato-infracao/incluir, /listar e /cancelar
/// </summary>
public class TesteInfracaoCredisisJdpi : IDisposable
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<TesteInfracaoCredisisJdpi> _logger;
    private string? _accessToken;

    // Configurações do credisis-jdpi-core
    private const string BASE_URL = "https://localhost:7189";
    private const string CLIENT_ID = "SCT01";
    private const string CLIENT_SECRET = "834737fcf9b3434da8262bd25650205e";

    public TesteInfracaoCredisisJdpi()
    {
        _httpClient = new HttpClient();
        _httpClient.BaseAddress = new Uri(BASE_URL);

        // Setup de logging
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder => builder.AddConsole());
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<TesteInfracaoCredisisJdpi>>();
    }

    /// <summary>
    /// Autentica no credisis-jdpi-core e obtém o token de acesso
    /// </summary>
    public async Task<bool> AutenticarAsync()
    {
        try
        {
            _logger.LogInformation("🔐 Iniciando autenticação no credisis-jdpi-core...");

            var tokenRequest = new List<KeyValuePair<string, string>>
            {
                new("client_id", CLIENT_ID),
                new("client_secret", CLIENT_SECRET),
                new("grant_type", "client_credentials"),
                new("scope", "dict_api,qrcode_api,spi_api")
            };

            var formContent = new FormUrlEncodedContent(tokenRequest);
            var response = await _httpClient.PostAsync("/api/auth/token", formContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                var tokenResponse = JsonSerializer.Deserialize<JsonElement>(responseContent);
                _accessToken = tokenResponse.GetProperty("access_token").GetString();
                
                _logger.LogInformation("✅ Autenticação realizada com sucesso!");
                return true;
            }
            else
            {
                _logger.LogError("❌ Falha na autenticação: {StatusCode} - {Content}", response.StatusCode, responseContent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro durante autenticação");
            return false;
        }
    }

    /// <summary>
    /// Testa a rota POST /api/infracoes/relato-infracao/incluir
    /// </summary>
    public async Task<bool> TestarIncluirRelatoInfracaoAsync()
    {
        try
        {
            _logger.LogInformation("📡 Testando POST /api/infracoes/relato-infracao/incluir...");

            // Exemplo de requisição baseado na documentação fornecida
            var requestBody = new
            {
                ispb = "04358798",
                endToEndId = "E9999901012341234123412345678900",
                motivo = 1,
                tpSitOrigem = 0,
                detalhes = "Transação feita através de QR Code falso em boleto",
                contatoCriador = new
                {
                    email = "fulano.tal@criador.com.br",
                    telefone = "+556198887777"
                }
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Headers obrigatórios
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
            _httpClient.DefaultRequestHeaders.Add("Chave-Idempotencia", Guid.NewGuid().ToString("N")[..36]);

            var response = await _httpClient.PostAsync("/api/infracoes/relato-infracao/incluir", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ Incluir relato de infração - SUCESSO!");
                _logger.LogInformation("📄 Resposta: {Response}", responseContent);
                
                // Validar estrutura da resposta esperada
                var responseObj = JsonSerializer.Deserialize<JsonElement>(responseContent);
                if (responseObj.TryGetProperty("idRelatoInfracao", out _) &&
                    responseObj.TryGetProperty("stRelatoInfracao", out _))
                {
                    _logger.LogInformation("✅ Estrutura da resposta está correta!");
                    return true;
                }
                else
                {
                    _logger.LogWarning("⚠️ Resposta não contém campos esperados");
                    return false;
                }
            }
            else
            {
                _logger.LogError("❌ Falha ao incluir relato: {StatusCode} - {Content}", response.StatusCode, responseContent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao testar incluir relato de infração");
            return false;
        }
    }

    /// <summary>
    /// Testa a rota GET /api/infracoes/relato-infracao/listar
    /// </summary>
    public async Task<bool> TestarListarRelatosInfracaoAsync()
    {
        try
        {
            _logger.LogInformation("📡 Testando GET /api/infracoes/relato-infracao/listar...");

            // Parâmetros de consulta
            var queryParams = new List<string>
            {
                "ispb=04358798",
                "ehRelatoSolicitado=false",
                "incluiDetalhes=true",
                "pagina=1",
                "tamanhoPagina=10"
            };

            var queryString = string.Join("&", queryParams);
            var url = $"/api/infracoes/relato-infracao/listar?{queryString}";

            // Headers obrigatórios
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");

            var response = await _httpClient.GetAsync(url);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ Listar relatos de infração - SUCESSO!");
                _logger.LogInformation("📄 Resposta: {Response}", responseContent);
                return true;
            }
            else
            {
                _logger.LogError("❌ Falha ao listar relatos: {StatusCode} - {Content}", response.StatusCode, responseContent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao testar listar relatos de infração");
            return false;
        }
    }

    /// <summary>
    /// Testa a rota POST /api/infracoes/relato-infracao/cancelar
    /// </summary>
    public async Task<bool> TestarCancelarRelatoInfracaoAsync(string idRelatoInfracao = "91d65e98-97c0-4b0f-b577-73625da1f9fc")
    {
        try
        {
            _logger.LogInformation("📡 Testando POST /api/infracoes/relato-infracao/cancelar...");

            var requestBody = new
            {
                idRelatoInfracao = idRelatoInfracao,
                ispb = "04358798"
            };

            var json = JsonSerializer.Serialize(requestBody);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Headers obrigatórios
            _httpClient.DefaultRequestHeaders.Clear();
            _httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_accessToken}");
            _httpClient.DefaultRequestHeaders.Add("Chave-Idempotencia", Guid.NewGuid().ToString("N")[..36]);

            var response = await _httpClient.PostAsync("/api/infracoes/relato-infracao/cancelar", content);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("✅ Cancelar relato de infração - SUCESSO!");
                _logger.LogInformation("📄 Resposta: {Response}", responseContent);
                return true;
            }
            else
            {
                _logger.LogError("❌ Falha ao cancelar relato: {StatusCode} - {Content}", response.StatusCode, responseContent);
                return false;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "❌ Erro ao testar cancelar relato de infração");
            return false;
        }
    }

    /// <summary>
    /// Executa todos os testes de infração em sequência
    /// </summary>
    public async Task<bool> ExecutarTodosTestesAsync()
    {
        _logger.LogInformation("🚀 INICIANDO TESTES DE INFRAÇÃO - credisis-jdpi-core");
        _logger.LogInformation("================================================================");

        // 1. Autenticação
        var autenticado = await AutenticarAsync();
        if (!autenticado)
        {
            _logger.LogError("❌ Falha na autenticação. Abortando testes.");
            return false;
        }

        // 2. Teste de inclusão
        var incluirSucesso = await TestarIncluirRelatoInfracaoAsync();
        
        // 3. Teste de listagem
        var listarSucesso = await TestarListarRelatosInfracaoAsync();
        
        // 4. Teste de cancelamento (usando ID fictício)
        var cancelarSucesso = await TestarCancelarRelatoInfracaoAsync();

        var todosTestesOk = incluirSucesso && listarSucesso && cancelarSucesso;
        
        _logger.LogInformation("================================================================");
        _logger.LogInformation("🏁 RESULTADO FINAL: {Resultado}", todosTestesOk ? "✅ TODOS OS TESTES PASSARAM!" : "❌ ALGUNS TESTES FALHARAM");
        
        return todosTestesOk;
    }

    public void Dispose()
    {
        _httpClient?.Dispose();
    }
}