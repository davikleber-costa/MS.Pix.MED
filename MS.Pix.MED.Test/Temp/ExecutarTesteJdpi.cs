using MS.Pix.MED.Test.Temp;

namespace MS.Pix.MED.Test.Temp;

/// <summary>
/// Classe para executar os testes de integração JDPI
/// Execute este arquivo para testar a integração
/// </summary>
public class ExecutarTesteJdpi
{
    /// <summary>
    /// Método principal para executar todos os testes
    /// BREAKPOINT: Coloque aqui para iniciar o debug completo
    /// </summary>
    public static async Task Main(string[] args)
    {
        Console.WriteLine("🚀 INICIANDO TESTES DE INTEGRAÇÃO MS.Pix.MED ↔ credisis-jdpi-core");
        Console.WriteLine("================================================================");
        
        var teste = new TesteIntegracaoJdpi();
        
        try
        {
            // BREAKPOINT 1: Verificar configurações
            Console.WriteLine("\n📋 1. VERIFICANDO CONFIGURAÇÕES...");
            teste.VerificarConfiguracoes();
            
            // BREAKPOINT 2: Teste de autenticação
            Console.WriteLine("\n🔐 2. TESTANDO AUTENTICAÇÃO...");
            var autenticado = await teste.TestarAutenticacaoAsync();
            
            if (autenticado)
            {
                Console.WriteLine("✅ Autenticação realizada com SUCESSO!");
                
                // BREAKPOINT 3: Teste de requisição completa
                Console.WriteLine("\n📡 3. TESTANDO REQUISIÇÃO COMPLETA...");
                var resultado = await teste.TestarRequisicaoCompletaAsync();
                
                if (resultado != null)
                {
                    Console.WriteLine("✅ Requisição completa realizada com SUCESSO!");
                }
                else
                {
                    Console.WriteLine("❌ Falha na requisição completa");
                }
            }
            else
            {
                Console.WriteLine("❌ FALHA na autenticação - Verifique se o credisis-jdpi-core está rodando na porta 7189");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 ERRO GERAL: {ex.Message}");
            Console.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
        
        Console.WriteLine("\n================================================================");
        Console.WriteLine("🏁 TESTES FINALIZADOS");
        Console.WriteLine("Pressione qualquer tecla para sair...");
        Console.ReadKey();
    }
    
    /// <summary>
    /// Teste individual de autenticação
    /// Use este método para testar apenas a autenticação
    /// </summary>
    public static async Task TestarApenasAutenticacao()
    {
        Console.WriteLine("🔐 TESTE INDIVIDUAL - APENAS AUTENTICAÇÃO");
        
        var teste = new TesteIntegracaoJdpi();
        
        // BREAKPOINT: Coloque aqui para debug específico de autenticação
        var resultado = await teste.TestarAutenticacaoAsync();
        
        Console.WriteLine(resultado ? "✅ Sucesso!" : "❌ Falha!");
    }
}