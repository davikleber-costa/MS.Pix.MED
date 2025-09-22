using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Pix.MED.Integration.Jdpi.Services;
using MS.Pix.MED.Integration.Jdpi.Configuration;
using System.Text.Json;

namespace MS.Pix.MED.Test.Temp;

/// <summary>
/// Classe de teste para verificar a integração com o credisis-jdpi-core
/// IMPORTANTE: Esta é uma classe temporária para testes de integração
/// </summary>
public class TesteIntegracaoJdpi
{
    private readonly JdpiIntegrationService _jdpiService;
    private readonly ILogger<TesteIntegracaoJdpi> _logger;

    public TesteIntegracaoJdpi()
    {
        // Configuração manual para testes
        var configuration = new JdpiConfiguration
        {
            ClientId = "SCT01",
            ClientSecret = "834737fcf9b3434da8262bd25650205e",
            BaseUrl = "https://localhost:7189",
            AuthUrl = "https://localhost:7189",
            Scope = "dict_api,qrcode_api,spi_api",
            EnableTokenCache = true,
            TokenCacheMinutes = 30
        };

        // Setup completo de DI para testes
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder => builder.AddConsole());
        serviceCollection.AddHttpClient();
        serviceCollection.Configure<JdpiConfiguration>(opt =>
        {
            opt.ClientId = configuration.ClientId;
            opt.ClientSecret = configuration.ClientSecret;
            opt.BaseUrl = configuration.BaseUrl;
            opt.AuthUrl = configuration.AuthUrl;
            opt.Scope = configuration.Scope;
            opt.EnableTokenCache = configuration.EnableTokenCache;
            opt.TokenCacheMinutes = configuration.TokenCacheMinutes;
        });
        
        var serviceProvider = serviceCollection.BuildServiceProvider();
        
        _logger = serviceProvider.GetRequiredService<ILogger<TesteIntegracaoJdpi>>();
        var httpClient = serviceProvider.GetRequiredService<HttpClient>();
        var options = serviceProvider.GetRequiredService<IOptions<JdpiConfiguration>>();
        var jdpiLogger = serviceProvider.GetRequiredService<ILogger<JdpiIntegrationService>>();
        
        _jdpiService = new JdpiIntegrationService(httpClient, options, jdpiLogger);
    }

    /// <summary>
    /// Teste principal de autenticação
    /// BREAKPOINT: Coloque aqui para iniciar o debug
    /// </summary>
    public async Task<bool> TestarAutenticacaoAsync()
    {
        try
        {
            Console.WriteLine("=== INICIANDO TESTE DE AUTENTICAÇÃO JDPI ===");
            _logger.LogInformation("Iniciando teste de autenticação com credisis-jdpi-core");

            // BREAKPOINT 1: Coloque aqui para ver o início
            var inicioTeste = DateTime.Now;
            
            // BREAKPOINT 2: F11 (Step Into) para entrar no JdpiIntegrationService.AuthenticateAsync()
            var autenticado = await _jdpiService.AuthenticateAsync();
            
            var fimTeste = DateTime.Now;
            var tempoDecorrido = fimTeste - inicioTeste;
            
            // BREAKPOINT 3: Coloque aqui para ver o resultado
            var resultado = new
            {
                Sucesso = autenticado,
                TempoDecorrido = tempoDecorrido.TotalMilliseconds,
                Timestamp = DateTime.Now,
                Mensagem = autenticado ? "Autenticação realizada com sucesso!" : "Falha na autenticação"
            };
            
            Console.WriteLine($"Resultado: {JsonSerializer.Serialize(resultado, new JsonSerializerOptions { WriteIndented = true })}");
            _logger.LogInformation("=== RESULTADO TESTE AUTENTICAÇÃO: {Resultado} ===", JsonSerializer.Serialize(resultado));
            
            return autenticado;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO: {ex.Message}");
            _logger.LogError(ex, "Erro durante teste de autenticação JDPI");
            return false;
        }
    }

    /// <summary>
    /// Teste de requisição completa (autenticação + chamada)
    /// </summary>
    public async Task<object?> TestarRequisicaoCompletaAsync()
    {
        try
        {
            Console.WriteLine("=== INICIANDO TESTE DE REQUISIÇÃO COMPLETA ===");
            _logger.LogInformation("Iniciando teste de requisição completa");
            
            // BREAKPOINT 4: Para ver requisição completa
            var dadosTeste = new
            {
                teste = true,
                timestamp = DateTime.Now,
                dados = "Teste de integração MS.Pix.MED -> credisis-jdpi-core"
            };
            
            var transacaoId = new Random().NextInt64(1000, 9999);
            
            // BREAKPOINT 5: F11 para acompanhar todo o fluxo
            var retorno = await _jdpiService.SendToJdpiAsync(
                "api/infracoes/relato-infracao/listar", 
                dadosTeste,
                transacaoId
            );
            
            // BREAKPOINT 6: Para ver a resposta do JDPI
            var resultado = new
            {
                TransacaoId = retorno.TransacaoId,
                RequisicaoEnviada = retorno.RequisicaoJdpi,
                RespostaRecebida = retorno.RespostaJdpi,
                DataCriacao = retorno.DataCriacao,
                Sucesso = !string.IsNullOrEmpty(retorno.RespostaJdpi)
            };
            
            Console.WriteLine($"Resultado Completo: {JsonSerializer.Serialize(resultado, new JsonSerializerOptions { WriteIndented = true })}");
            return resultado;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ERRO na requisição completa: {ex.Message}");
            _logger.LogError(ex, "Erro durante teste de requisição completa");
            return null;
        }
    }

    /// <summary>
    /// Método para verificar configurações
    /// </summary>
    public void VerificarConfiguracoes()
    {
        // BREAKPOINT 7: Para inspecionar configurações
        var config = new
        {
            Mensagem = "Configurações do JDPI para teste",
            ConfiguracaoAtual = new
            {
                ClientId = "SCT01",
                ClientSecret = "834737fcf9b3434da8262bd25650205e",
                BaseUrl = "https://localhost:7189",
                AuthUrl = "https://localhost:7189",
                Scope = "dict_api,qrcode_api,spi_api"
            },
            InstrucoesDebug = new[]
            {
                "1. Execute o projeto credisis-jdpi-core na porta 7189",
                "2. Coloque breakpoint no método TestarAutenticacaoAsync()",
                "3. Execute este teste em modo debug",
                "4. Use F11 para entrar no JdpiIntegrationService.AuthenticateAsync()",
                "5. Inspecione as variáveis: _configuration, response, tokenResponse, _accessToken"
            }
        };
        
        Console.WriteLine($"Configurações: {JsonSerializer.Serialize(config, new JsonSerializerOptions { WriteIndented = true })}");
    }
}