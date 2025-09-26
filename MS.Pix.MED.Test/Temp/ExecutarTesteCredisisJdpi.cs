using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MS.Pix.MED.Test.Temp;

/// <summary>
/// Executor principal para testes de integração com credisis-jdpi-core
/// Substitui o ExecutarTesteJdpi.cs para focar nas rotas de infração
/// </summary>
public class ExecutarTesteCredisisJdpi
{
    private readonly ILogger<ExecutarTesteCredisisJdpi> _logger;

    public ExecutarTesteCredisisJdpi()
    {
        // Setup de logging
        var serviceCollection = new ServiceCollection();
        serviceCollection.AddLogging(builder => builder.AddConsole());
        var serviceProvider = serviceCollection.BuildServiceProvider();
        _logger = serviceProvider.GetRequiredService<ILogger<ExecutarTesteCredisisJdpi>>();
    }

    /// <summary>
    /// Executa apenas o teste de autenticação
    /// </summary>
    public async Task ExecutarApenasAutenticacaoAsync()
    {
        _logger.LogInformation("🔐 EXECUTANDO APENAS TESTE DE AUTENTICAÇÃO");
        _logger.LogInformation("================================================");

        using var teste = new TesteInfracaoCredisisJdpi();
        var sucesso = await teste.AutenticarAsync();

        _logger.LogInformation("================================================");
        _logger.LogInformation("🏁 RESULTADO: {Resultado}", sucesso ? "✅ AUTENTICAÇÃO OK" : "❌ FALHA NA AUTENTICAÇÃO");
    }

    /// <summary>
    /// Executa todos os testes de infração (autenticação + rotas)
    /// </summary>
    public async Task ExecutarTestesCompletosAsync()
    {
        _logger.LogInformation("🚀 EXECUTANDO TESTES COMPLETOS DE INFRAÇÃO");
        _logger.LogInformation("==============================================");

        using var teste = new TesteInfracaoCredisisJdpi();
        var sucesso = await teste.ExecutarTodosTestesAsync();

        _logger.LogInformation("==============================================");
        _logger.LogInformation("🏁 RESULTADO FINAL: {Resultado}", sucesso ? "✅ TODOS OS TESTES OK" : "❌ ALGUNS TESTES FALHARAM");
    }

    /// <summary>
    /// Executa teste específico de inclusão de relato
    /// </summary>
    public async Task ExecutarTesteInclusaoAsync()
    {
        _logger.LogInformation("📝 EXECUTANDO TESTE DE INCLUSÃO DE RELATO");
        _logger.LogInformation("==========================================");

        using var teste = new TesteInfracaoCredisisJdpi();
        
        // Autentica primeiro
        var autenticado = await teste.AutenticarAsync();
        if (!autenticado)
        {
            _logger.LogError("❌ Falha na autenticação. Abortando teste.");
            return;
        }

        // Executa teste de inclusão
        var sucesso = await teste.TestarIncluirRelatoInfracaoAsync();

        _logger.LogInformation("==========================================");
        _logger.LogInformation("🏁 RESULTADO: {Resultado}", sucesso ? "✅ INCLUSÃO OK" : "❌ FALHA NA INCLUSÃO");
    }

    /// <summary>
    /// Executa teste específico de listagem de relatos
    /// </summary>
    public async Task ExecutarTesteListagemAsync()
    {
        _logger.LogInformation("📋 EXECUTANDO TESTE DE LISTAGEM DE RELATOS");
        _logger.LogInformation("===========================================");

        using var teste = new TesteInfracaoCredisisJdpi();
        
        // Autentica primeiro
        var autenticado = await teste.AutenticarAsync();
        if (!autenticado)
        {
            _logger.LogError("❌ Falha na autenticação. Abortando teste.");
            return;
        }

        // Executa teste de listagem
        var sucesso = await teste.TestarListarRelatosInfracaoAsync();

        _logger.LogInformation("===========================================");
        _logger.LogInformation("🏁 RESULTADO: {Resultado}", sucesso ? "✅ LISTAGEM OK" : "❌ FALHA NA LISTAGEM");
    }

    /// <summary>
    /// Método principal para execução via console ou debug
    /// </summary>
    public static async Task Main(string[] args)
    {
        var executor = new ExecutarTesteCredisisJdpi();

        try
        {
            // Verifica argumentos da linha de comando
            if (args.Length > 0)
            {
                switch (args[0].ToLower())
                {
                    case "auth":
                    case "autenticacao":
                        await executor.ExecutarApenasAutenticacaoAsync();
                        break;
                    case "incluir":
                    case "inclusao":
                        await executor.ExecutarTesteInclusaoAsync();
                        break;
                    case "listar":
                    case "listagem":
                        await executor.ExecutarTesteListagemAsync();
                        break;
                    case "completo":
                    case "todos":
                    default:
                        await executor.ExecutarTestesCompletosAsync();
                        break;
                }
            }
            else
            {
                // Execução padrão - todos os testes
                await executor.ExecutarTestesCompletosAsync();
            }
        }
        catch (Exception ex)
        {
            executor._logger.LogError(ex, "❌ Erro durante execução dos testes");
        }

        // Pausa para visualizar resultados no console
        Console.WriteLine("\nPressione qualquer tecla para sair...");
        Console.ReadKey();
    }
}