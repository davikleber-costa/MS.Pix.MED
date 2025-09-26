using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using MS.Pix.MED.Domain.DTOs;
using MS.Pix.MED.Domain.Interfaces;
using MS.Pix.MED.Integration.Jdpi.Services;

namespace MS.Pix.MED.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InfracaoMedController : MedControllerBase
{
    private readonly JdpiIntegrationService _jdpiService;
    private readonly IPixTransacaoRepository _pixTransacaoRepository;
    private readonly IConfiguration _configuration;

    public InfracaoMedController(
        JdpiIntegrationService jdpiService,
        IPixTransacaoRepository pixTransacaoRepository,
        IConfiguration configuration)
    {
        _jdpiService = jdpiService;
        _pixTransacaoRepository = pixTransacaoRepository;
        _configuration = configuration;
    }


    [HttpPost("contestacao-med/incluir")]
    public async Task<IActionResult> IncluirContestacaoMed(
        [FromHeader(Name = "Chave-Idempotencia")] string? idempotencia,
        [FromBody] ContestacaoMedDto request,
        CancellationToken ct)
    {
        try
        {
            // Buscar dados da transação no MySQL usando o comprovante_id
            var pixTransacao = await _pixTransacaoRepository.GetByComprovanteIdAsync(request.ComprovanteId);
            
            if (pixTransacao == null)
            {
                return BadRequest($"Transação não encontrada para o comprovante_id: {request.ComprovanteId}");
            }

            // Obter dados de contato do appsettings
            var contatoCriador = _configuration.GetSection("ContatoCriador");
            var email = contatoCriador["Email"];
            var telefone = contatoCriador["Telefone"];

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(telefone))
            {
                return BadRequest("Configurações de ContatoCriador não encontradas no appsettings");
            }

            // Montar o JSON para o credisis-jdpi-core
            var incluirRelatoRequest = new IncluirRelatoInfracaoRequest
            {
                Ispb = pixTransacao.IspbOrigem,
                EndToEndId = pixTransacao.EndToEndId,
                Motivo = 1, // Estático: 1 = Solicitação de devolução
                TpSitOrigem = request.TpSitOrigem, // Vem do frontend (0 a 4)
                Detalhes = request.Detalhes ?? pixTransacao.ValorDetalhes,
                ContatoCriador = new ContatoCriadorDto
                {
                    Email = email,
                    Telefone = telefone
                }
            };

            // Enviar para o credisis-jdpi-core
            var result = await _jdpiService.SendToJdpiAsync(
                "api/infracoes/relato-infracao/incluir", 
                incluirRelatoRequest, 
                0); // Usar 0 como transacaoId

            // Verifica se a resposta contém erro
            if (!string.IsNullOrEmpty(result.RespostaJdpi) && !result.RespostaJdpi.StartsWith("Erro:"))
            {
                return Ok(result.RespostaJdpi);
            }
            else
            {
                return Problem($"Erro ao incluir relato de infração: {result.RespostaJdpi}", statusCode: 500);
            }
        }
        catch (Exception ex)
        {
            return Problem($"Erro interno: {ex.Message}", statusCode: 500);
        }
    }

    [HttpGet("contestacao-med/listar")]
    public async Task<IActionResult> ListarContestacoesMed(
        [FromQuery] ListarRelatosInfracaoQuery query,
        CancellationToken ct)
    {
        try
        {
            // Monta a query string para o endpoint de listagem
            var queryParams = new List<string>
            {
                $"ispb={query.Ispb}",
                $"ehRelatoSolicitado={query.EhRelatoSolicitado}",
                $"incluiDetalhes={query.IncluiDetalhes}",
                $"pagina={query.Pagina}",
                $"tamanhoPagina={query.TamanhoPagina}"
            };

            if (query.StRelatoInfracao.HasValue)
                queryParams.Add($"stRelatoInfracao={query.StRelatoInfracao}");

            if (query.DtHrModificacaoInicio.HasValue)
                queryParams.Add($"dtHrModificacaoInicio={query.DtHrModificacaoInicio:yyyy-MM-ddTHH:mm:ssZ}");

            if (query.DtHrModificacaoFim.HasValue)
                queryParams.Add($"dtHrModificacaoFim={query.DtHrModificacaoFim:yyyy-MM-ddTHH:mm:ssZ}");

            var endpoint = $"api/infracoes/relato-infracao/listar?{string.Join("&", queryParams)}";

            // Chama o endpoint de listagem no credisis-jdpi-core
            var result = await _jdpiService.SendToJdpiAsync(
                endpoint, 
                null!, // GET não precisa de body
                0); // Usar 0 como transacaoId

            // Verifica se a resposta contém erro
            if (!string.IsNullOrEmpty(result.RespostaJdpi) && !result.RespostaJdpi.StartsWith("Erro:"))
            {
                return Ok(result.RespostaJdpi);
            }
            else
            {
                return Problem($"Erro ao listar relatos de infração: {result.RespostaJdpi}", statusCode: 500);
            }
        }
        catch (Exception ex)
        {
            return Problem($"Erro interno: {ex.Message}", statusCode: 500);
        }
    }

    [HttpPost("contestacao-med/cancelar")]
    public async Task<IActionResult> CancelarContestacaoMed(
        [FromBody] CancelarRelatoInfracaoRequest body,
        CancellationToken ct)
    {
        try
        {
            // Chama o endpoint de cancelamento no credisis-jdpi-core
            var result = await _jdpiService.SendToJdpiAsync(
                "api/infracoes/relato-infracao/cancelar", 
                body, 
                0); // Usar 0 como transacaoId

            // Verifica se a resposta contém erro
            if (!string.IsNullOrEmpty(result.RespostaJdpi) && !result.RespostaJdpi.StartsWith("Erro:"))
            {
                return Ok(result.RespostaJdpi);
            }
            else
            {
                return Problem($"Erro ao cancelar relato de infração: {result.RespostaJdpi}", statusCode: 500);
            }
        }
        catch (Exception ex)
        {
            return Problem($"Erro interno: {ex.Message}", statusCode: 500);
        }
    }
}