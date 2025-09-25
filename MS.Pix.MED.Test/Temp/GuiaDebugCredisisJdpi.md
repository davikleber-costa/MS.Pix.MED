# Guia de Debug - Integração credisis-jdpi-core

## 📁 Localização dos Arquivos de Teste

- **Pasta:** `MS.Pix.MED.Test\Temp\`
- **Arquivos principais:**
  - `TesteInfracaoCredisisJdpi.cs` - Classe principal de testes
  - `ExecutarTesteCredisisJdpi.cs` - Executor dos testes
  - `ExemplosJsonCredisis.json` - Exemplos de requisições e respostas
  - `GuiaDebugCredisisJdpi.md` - Este guia

## 🚀 Como Executar os Testes

### Opção 1: Teste Completo
```csharp
var executor = new ExecutarTesteCredisisJdpi();
await executor.ExecutarTestesCompletosAsync();
```

### Opção 2: Apenas Autenticação
```csharp
var executor = new ExecutarTesteCredisisJdpi();
await executor.ExecutarApenasAutenticacaoAsync();
```

### Opção 3: Teste Específico
```csharp
var executor = new ExecutarTesteCredisisJdpi();
await executor.ExecutarTesteInclusaoAsync();     // Só inclusão
await executor.ExecutarTesteListagemAsync();     // Só listagem
```

### Opção 4: Via Linha de Comando
```bash
dotnet run -- auth        # Apenas autenticação
dotnet run -- incluir     # Teste de inclusão
dotnet run -- listar      # Teste de listagem  
dotnet run -- completo    # Todos os testes
```

## 🔍 Pontos de Interrupção Essenciais

### 1. ExecutarTesteCredisisJdpi.cs
- **Linha ~25:** `var sucesso = await teste.AutenticarAsync();`
- **Linha ~45:** `var sucesso = await teste.ExecutarTodosTestesAsync();`
- **Linha ~65:** `var sucesso = await teste.TestarIncluirRelatoInfracaoAsync();`

### 2. TesteInfracaoCredisisJdpi.cs
- **Linha ~45:** `var response = await _httpClient.PostAsync("/connect/token", formContent);`
- **Linha ~85:** `_httpClient.DefaultRequestHeaders.Add("Chave-Idempotencia", ...);`
- **Linha ~90:** `var response = await _httpClient.PostAsync("/api/infracoes/relato-infracao/incluir", content);`
- **Linha ~140:** `var response = await _httpClient.GetAsync(url);`
- **Linha ~190:** `var response = await _httpClient.PostAsync("/api/infracoes/relato-infracao/cancelar", content);`

## 🔧 Variáveis Importantes para Inspeção

### Durante Autenticação
- `tokenRequest` - Parâmetros da requisição de token
- `response.StatusCode` - Status da resposta (deve ser 200)
- `_accessToken` - Token obtido (deve estar preenchido)

### Durante Inclusão de Relato
- `requestBody` - Corpo da requisição JSON
- `_httpClient.DefaultRequestHeaders` - Headers (Authorization + Chave-Idempotencia)
- `response.StatusCode` - Status da resposta (deve ser 200)
- `responseContent` - Resposta JSON com idRelatoInfracao

### Durante Listagem
- `queryString` - Parâmetros de consulta
- `url` - URL completa com query parameters
- `responseContent` - Lista de relatos retornados

### Durante Cancelamento
- `requestBody.idRelatoInfracao` - ID do relato a cancelar
- `response.StatusCode` - Status da resposta (deve ser 200)

## 🌐 Configurações de Ambiente

### URLs Base
- **Desenvolvimento:** `https://localhost:7189`
- **Homologação:** `https://credisis-jdpi-hml.example.com`
- **Produção:** `https://credisis-jdpi-prod.example.com`

### Credenciais (Desenvolvimento)
- **Client ID:** `SCT01`
- **Client Secret:** `834737fcf9b3434da8262bd25650205e`
- **Scopes:** `dict_api,qrcode_api,spi_api`

## 📋 Headers Obrigatórios

### Para Autenticação (/connect/token)
```
Content-Type: application/x-www-form-urlencoded
```

### Para Rotas de Infração
```
Authorization: Bearer {access_token}
Chave-Idempotencia: {guid_36_chars}  // Apenas para POST
Content-Type: application/json       // Apenas para POST
```

## 🐛 Problemas Comuns e Soluções

### 1. Erro 401 - Unauthorized
- **Causa:** Token inválido ou expirado
- **Solução:** Verificar se a autenticação foi bem-sucedida e o token está sendo enviado corretamente

### 2. Erro 400 - Bad Request
- **Causa:** Headers obrigatórios ausentes ou formato JSON inválido
- **Solução:** Verificar se `Chave-Idempotencia` está presente e o JSON está bem formado

### 3. Erro 422 - Unprocessable Entity
- **Causa:** Dados de entrada inválidos
- **Solução:** Verificar se os campos obrigatórios estão preenchidos e os valores estão no formato correto

### 4. Timeout de Conexão
- **Causa:** Serviço credisis-jdpi-core não está rodando
- **Solução:** Verificar se o serviço está ativo na URL configurada

## 📊 Estrutura de Resposta Esperada

### Inclusão de Relato (200 OK)
```json
{
  "idRelatoInfracao": "91d65e98-97c0-4b0f-b577-73625da1f9fc",
  "stRelatoInfracao": "0",
  "pspCriador": "04358798",
  "pspContraParte": "99999011",
  "dthrCriacaoRelato": "2023-08-22T10:30:10.008Z",
  "dthrUltimaModificacao": "2023-08-22T10:30:10.008Z"
}
```

### Listagem de Relatos (200 OK)
```json
{
  "relatos": [...],
  "totalRegistros": 1,
  "paginaAtual": 1,
  "totalPaginas": 1
}
```

## 🔄 Fluxo de Teste Recomendado

1. **Autenticação** → Obter token de acesso
2. **Inclusão** → Criar novo relato de infração
3. **Listagem** → Verificar se o relato foi criado
4. **Cancelamento** → Cancelar o relato criado (opcional)

## 📝 Logs Importantes

Os testes geram logs detalhados com emojis para facilitar a identificação:
- 🔐 Autenticação
- 📡 Requisições HTTP
- ✅ Sucessos
- ❌ Erros
- ⚠️ Avisos
- 🏁 Resultados finais

## 🔗 Integração com MS.Pix.MED

Os testes verificam a integração entre:
- **MS.Pix.MED** (controllers que herdam de MedControllerBase)
- **credisis-jdpi-core** (rotas de infração)

Certifique-se de que ambos os serviços estejam rodando durante os testes de integração completa.