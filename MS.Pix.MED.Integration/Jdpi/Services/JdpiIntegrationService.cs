using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Integration.Jdpi.Configuration;
using MS.Pix.MED.Application.RetornoJdpi.Commands;
using MS.Pix.MED.Application.Transacao.Commands;
using MediatR;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Globalization;

namespace MS.Pix.MED.Integration.Jdpi.Services;

public class JdpiIntegrationService
{
    private readonly HttpClient _httpClient;
    private readonly JdpiConfiguration _configuration;
    private readonly ILogger<JdpiIntegrationService> _logger;
    private readonly IMediator _mediator;

    public JdpiIntegrationService(
        HttpClient httpClient,
        IOptions<JdpiConfiguration> configuration,
        ILogger<JdpiIntegrationService> logger,
        IMediator mediator)
    {
        _httpClient = httpClient;
        _configuration = configuration.Value;
        _logger = logger;
        _mediator = mediator;
        
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

    public async Task<RetornoJdpi> SendToJdpiAsync(string endpoint, object requestData, long transacaoId)
    {
        var requestJson = JsonConvert.SerializeObject(requestData);
        string responseContent = string.Empty;
        DateTime? dataCriacaoRelato = null;
        TimeSpan? horaCriacaoRelato = null;
        bool isError = false;
        string errorMessage = string.Empty;
        long finalTransacaoId = transacaoId;

        try
        {
            _logger.LogInformation("Enviando dados para API JDPI externa - Endpoint: {Endpoint}", endpoint);

            var content = new StringContent(requestJson, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(endpoint, content);
            responseContent = await response.Content.ReadAsStringAsync();

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Dados enviados com sucesso para API JDPI externa");
                
                // Extrair dtHrCriacaoRelatoInfracao da resposta se for um sucesso (status 200)
                var (dataExtraida, horaExtraida) = ExtractDtHrCriacaoRelatoInfracao(responseContent);
                dataCriacaoRelato = dataExtraida;
                horaCriacaoRelato = horaExtraida;
            }
            else
            {
                _logger.LogWarning("Resposta de erro da API JDPI externa: {StatusCode} - {Content}", response.StatusCode, responseContent);
                isError = true;
                errorMessage = $"HTTP {response.StatusCode}: {responseContent}";
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao enviar dados para API JDPI externa");
            isError = true;
            errorMessage = ex.Message;
            responseContent = $"Erro: {ex.Message}";
        }

        // Se houve erro e transacaoId é 0, criar transação genérica de erro
        if (isError && transacaoId == 0)
        {
            try
            {
                // Extrair informações do erro para criar a transação
                var (idNotificacao, statusRelato, guidExtrato) = ExtractErrorInfo(responseContent, errorMessage);
                
                var createTransacaoCommand = new CreateTransacaoCommand(
                    TipoInfracaoId: 5, // ID para transação de erro
                    IdNotificacaoJdpi: idNotificacao,
                    StatusRelatoJdpi: statusRelato,
                    GuidExtratoJdpi: guidExtrato,
                    CaminhoArquivo: null,
                    DataCriacaoRelato: dataCriacaoRelato,
                    HoraCriacaoRelato: horaCriacaoRelato?.ToTimeOnly()
                );

                var transacaoCriada = await _mediator.Send(createTransacaoCommand);
                finalTransacaoId = transacaoCriada.Id;
                
                _logger.LogInformation("Transação genérica de erro criada com ID: {TransacaoId}", finalTransacaoId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Erro ao criar transação genérica de erro");
                // Se não conseguir criar a transação, usar ID 0 mesmo
                finalTransacaoId = 0;
            }
        }

        // Persistir no banco usando CQRS com data e hora extraídas (se disponíveis)
        var createCommand = new CreateRetornoJdpiCommand(
            finalTransacaoId,
            requestJson,
            responseContent,
            dataCriacaoRelato,
            horaCriacaoRelato // Já é TimeSpan?, não precisa converter
        );

        var retornoJdpi = await _mediator.Send(createCommand);
        return retornoJdpi;
    }

    /// <summary>
    /// Extrai informações de erro para criar transação genérica
    /// </summary>
    /// <param name="responseContent">Conteúdo da resposta</param>
    /// <param name="errorMessage">Mensagem de erro</param>
    /// <returns>Tupla com IdNotificacaoJdpi, StatusRelatoJdpi e GuidExtratoJdpi</returns>
    private (string idNotificacao, bool statusRelato, string guidExtrato) ExtractErrorInfo(string responseContent, string errorMessage)
    {
        try
        {
            // Tentar extrair informações do JSON de resposta se possível
            if (!string.IsNullOrWhiteSpace(responseContent) && responseContent.StartsWith("{"))
            {
                var jsonObject = JObject.Parse(responseContent);
                
                var idNotificacao = jsonObject.SelectToken("idNotificacaoJdpi")?.ToString() ?? 
                                   jsonObject.SelectToken("idRelatoInfracao")?.ToString() ?? 
                                   Guid.NewGuid().ToString("N");
                
                var guidExtrato = jsonObject.SelectToken("guidExtratoJdpi")?.ToString() ?? 
                                 jsonObject.SelectToken("guid")?.ToString() ?? 
                                 Guid.NewGuid().ToString("N");
                
                return (idNotificacao, false, guidExtrato); // StatusRelatoJdpi = false para erro
            }
        }
        catch (Exception ex)
        {
            _logger.LogWarning(ex, "Não foi possível extrair informações do JSON de erro");
        }

        // Se não conseguir extrair do JSON, gerar valores padrão
        return (
            idNotificacao: Guid.NewGuid().ToString("N"),
            statusRelato: false, // false indica erro
            guidExtrato: Guid.NewGuid().ToString("N")
        );
    }

    /// <summary>
    /// Extrai dtHrCriacaoRelatoInfracao da resposta JSON do JDPI
    /// </summary>
    /// <param name="responseContent">Conteúdo da resposta JSON</param>
    /// <returns>Tupla com data e hora extraídas, ou null se não encontradas</returns>
    private (DateTime? data, TimeSpan? hora) ExtractDtHrCriacaoRelatoInfracao(string responseContent)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(responseContent))
                return (null, null);

            var jsonObject = JObject.Parse(responseContent);
            var dtHrCriacaoToken = jsonObject.SelectToken("dtHrCriacaoRelatoInfracao");
            
            if (dtHrCriacaoToken == null)
                return (null, null);

            var dtHrCriacaoStr = dtHrCriacaoToken.ToString();
            
            // Tentar parsear diferentes formatos de data/hora
            if (DateTime.TryParse(dtHrCriacaoStr, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
            {
                return (dateTime.Date, dateTime.TimeOfDay);
            }
            
            // Tentar formato ISO 8601
            if (DateTime.TryParseExact(dtHrCriacaoStr, "yyyy-MM-ddTHH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeIso))
            {
                return (dateTimeIso.Date, dateTimeIso.TimeOfDay);
            }
            
            // Tentar formato ISO 8601 com milissegundos
            if (DateTime.TryParseExact(dtHrCriacaoStr, "yyyy-MM-ddTHH:mm:ss.fff", CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTimeIsoMs))
            {
                return (dateTimeIsoMs.Date, dateTimeIsoMs.TimeOfDay);
            }

            _logger.LogWarning("Não foi possível parsear dtHrCriacaoRelatoInfracao: {DtHrCriacao}", dtHrCriacaoStr);
            return (null, null);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao extrair dtHrCriacaoRelatoInfracao da resposta JDPI");
            return (null, null);
        }
    }

    public async Task<TResponse> QueryJdpiAsync<TResponse>(string endpoint, object? queryParameters = null)
    {
        try
        {
            _logger.LogInformation("Consultando dados da API JDPI externa - Endpoint: {Endpoint}", endpoint);

            var queryString = BuildQueryString(queryParameters);
            var url = $"{endpoint}{queryString}";

            var response = await _httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<TResponse>(responseContent) ?? default(TResponse)!;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro ao consultar dados da API JDPI externa");
            throw;
        }
    }

    public async Task<RetornoJdpi> ProcessRefundAsync(object refundData, long transacaoId)
    {
        return await SendToJdpiAsync("devolucao", refundData, transacaoId);
    }

    public async Task<TResponse> ListRefundsAsync<TResponse>(object? filters = null)
    {
        return await QueryJdpiAsync<TResponse>("devolucao/listar", filters);
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

// Extension method para converter TimeSpan para TimeOnly
public static class TimeSpanExtensions
{
    public static TimeOnly ToTimeOnly(this TimeSpan timeSpan)
    {
        return TimeOnly.FromTimeSpan(timeSpan);
    }
}