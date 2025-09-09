using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Api.Controllers;

[ApiController]
[Authorize]
[Route("v1/med/movimentacao")]
[Produces("application/json")]
public class MovimentacaoController : MedControllerBase
{
    [HttpPost]
    [ProducesResponseType<Movimentacao>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarMovimentacaoAsync([FromBody] Movimentacao movimentacao, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de criação usando MediatR ou serviço
        // Por enquanto retorna Created como placeholder
        return CreatedAtAction(nameof(ObterMovimentacaoPorIdAsync), new { id = movimentacao.IdMovimentacao }, movimentacao);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<Movimentacao>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMovimentacaoPorIdAsync(int id, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna NotFound como placeholder
        return NotFound($"Movimentação com ID {id} não encontrado");
    }

    [HttpGet("extrato/{idExtrato:guid}")]
    [ProducesResponseType<IEnumerable<Movimentacao>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMovimentacoesPorExtratoAsync(Guid idExtrato, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna lista vazia como placeholder
        return Ok(new List<Movimentacao>());
    }

    [HttpGet("tipo-infracao/{idTipoInfracao:int}")]
    [ProducesResponseType<IEnumerable<Movimentacao>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMovimentacoesPorTipoInfracaoAsync(int idTipoInfracao, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna lista vazia como placeholder
        return Ok(new List<Movimentacao>());
    }

    [HttpGet("relato/{idRelatoInfracao}")]
    [ProducesResponseType<IEnumerable<Movimentacao>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterMovimentacoesPorRelatoAsync(string idRelatoInfracao, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna lista vazia como placeholder
        return Ok(new List<Movimentacao>());
    }
}