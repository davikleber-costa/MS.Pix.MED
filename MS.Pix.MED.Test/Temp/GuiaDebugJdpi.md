# 🔍 Guia de Debug - Integração JDPI (MS.Pix.MED.Test/Temp)

## 📂 Localização dos Arquivos de Teste

- **Pasta:** `d:\SIS\MS.Pix.MED\MS.Pix.MED.Test\Temp\`
- **Arquivos:**
  - `TesteIntegracaoJdpi.cs` - Classe principal de teste
  - `ExecutarTesteJdpi.cs` - Executador dos testes
  - `GuiaDebugJdpi.md` - Este guia

## 🎯 Como Executar os Testes

### Opção 1: Executar Teste Completo
1. Abra o Visual Studio
2. Vá para `ExecutarTesteJdpi.cs`
3. Coloque breakpoint no método `Main`
4. Execute em modo debug (F5)

### Opção 2: Executar Apenas Autenticação
1. Vá para `ExecutarTesteJdpi.cs`
2. Coloque breakpoint no método `TestarApenasAutenticacao`
3. Execute este método específico

## 📍 Breakpoints Essenciais

### 1. **ExecutarTesteJdpi.cs**

```csharp
// BREAKPOINT 1 - Linha ~20: Início dos testes
public static async Task Main(string[] args)

// BREAKPOINT 2 - Linha ~28: Verificar configurações
teste.VerificarConfiguracoes();

// BREAKPOINT 3 - Linha ~32: Teste de autenticação
var autenticado = await teste.TestarAutenticacaoAsync();

// BREAKPOINT 4 - Linha ~39: Teste completo (se autenticação OK)
var resultado = await teste.TestarRequisicaoCompletaAsync();
```

### 2. **TesteIntegracaoJdpi.cs**

```csharp
// BREAKPOINT 5 - Linha ~35: Início do teste de autenticação
public async Task<bool> TestarAutenticacaoAsync()

// BREAKPOINT 6 - Linha ~42: Antes da chamada de autenticação
var inicioTeste = DateTime.Now;

// BREAKPOINT 7 - Linha ~45: Chamada principal (USE F11 AQUI!)
var autenticado = await _jdpiService.AuthenticateAsync();
// ⚠️ IMPORTANTE: Use F11 (Step Into) para entrar no JdpiIntegrationService

// BREAKPOINT 8 - Linha ~50: Verificar resultado
var resultado = new { ... };
```

### 3. **JdpiIntegrationService.cs** (Arquivo original do projeto)

```csharp
// BREAKPOINT 9 - Linha 44: Início do AuthenticateAsync
public async Task<bool> AuthenticateAsync()

// BREAKPOINT 10 - Linha 70: Criar requisição de token
var tokenRequest = new TokenRequest { ... };

// BREAKPOINT 11 - Linha 82: Chamada HTTP para /connect/token
var response = await authClient.PostAsync("/connect/token", formContent);

// BREAKPOINT 12 - Linha 83: Resposta do JDPI
var responseContent = await response.Content.ReadAsStringAsync();

// BREAKPOINT 13 - Linha 87: Deserializar token
var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);

// BREAKPOINT 14 - Linha 90: Armazenar token
_accessToken = tokenResponse.AccessToken;
```

## 🔍 Variáveis Importantes para Inspecionar

### Durante o Debug, inspecione estas variáveis:

1. **No TesteIntegracaoJdpi:**
   - `configuration` (configurações do teste)
   - `autenticado` (resultado da autenticação)
   - `resultado` (objeto com detalhes do teste)

2. **No JdpiIntegrationService:**
   - `_configuration.ClientId` → "SCT01"
   - `_configuration.ClientSecret` → "834737fcf9b3434da8262bd25650205e"
   - `_configuration.AuthUrl` → "https://localhost:7189"
   - `tokenRequest` (dados enviados)
   - `response.StatusCode` (deve ser 200)
   - `responseContent` (JSON da resposta)
   - `tokenResponse.AccessToken` (o token JWT)
   - `_accessToken` (token armazenado)

## 🚀 Pré-requisitos

### Antes de executar os testes:

1. ✅ **Execute o credisis-jdpi-core na porta 7189**
   ```bash
   cd d:\SIS\credisis-jdpi-core
   dotnet run --project src/JDPI.API
   ```

2. ✅ **Verifique se o JDPI está respondendo:**
   - Abra: `https://localhost:7189/swagger`
   - Deve mostrar a documentação da API

3. ✅ **Configure o MS.Pix.MED (se necessário):**
   - Verifique `appsettings.json`
   - Confirme as configurações do JDPI

## 🎮 Comandos de Debug no Visual Studio

- **F5** - Iniciar debug
- **F9** - Adicionar/remover breakpoint
- **F10** - Executar próxima linha (Step Over)
- **F11** - Entrar no método (Step Into) ⭐ **IMPORTANTE**
- **Shift+F11** - Sair do método (Step Out)
- **Ctrl+F5** - Executar sem debug

## 📊 Janelas Importantes do Visual Studio

1. **Locals** - Variáveis locais do método atual
2. **Watch** - Variáveis que você quer monitorar
3. **Call Stack** - Pilha de chamadas
4. **Output** - Logs e saídas do console
5. **Immediate Window** - Execute comandos durante debug

## 🐛 Problemas Comuns

### ❌ "Connection refused"
- **Causa:** credisis-jdpi-core não está rodando
- **Solução:** Execute o JDPI na porta 7189

### ❌ "Unauthorized" (401)
- **Causa:** Credenciais incorretas
- **Solução:** Verificar ClientId/ClientSecret

### ❌ "Invalid scope"
- **Causa:** Scope incorreto
- **Solução:** Usar "dict_api,qrcode_api,spi_api"

### ❌ Breakpoints não funcionam
- **Causa:** Projeto não está em modo Debug
- **Solução:** Compilar em Debug mode

## 📝 Exemplo de Saída Esperada

```
🚀 INICIANDO TESTES DE INTEGRAÇÃO MS.Pix.MED ↔ credisis-jdpi-core
================================================================

📋 1. VERIFICANDO CONFIGURAÇÕES...
Configurações: {
  "Mensagem": "Configurações do JDPI para teste",
  "ConfiguracaoAtual": {
    "ClientId": "SCT01",
    "ClientSecret": "834737fcf9b3434da8262bd25650205e",
    "BaseUrl": "https://localhost:7189",
    "AuthUrl": "https://localhost:7189",
    "Scope": "dict_api,qrcode_api,spi_api"
  }
}

🔐 2. TESTANDO AUTENTICAÇÃO...
=== INICIANDO TESTE DE AUTENTICAÇÃO JDPI ===
Resultado: {
  "Sucesso": true,
  "TempoDecorrido": 1234.56,
  "Timestamp": "2024-01-20T10:30:00",
  "Mensagem": "Autenticação realizada com sucesso!"
}
✅ Autenticação realizada com SUCESSO!

📡 3. TESTANDO REQUISIÇÃO COMPLETA...
✅ Requisição completa realizada com SUCESSO!

================================================================
🏁 TESTES FINALIZADOS
```

---

**🎯 Objetivo:** Verificar se o MS.Pix.MED consegue obter token do credisis-jdpi-core e visualizar esse token durante o debug dentro do escopo do projeto de testes.