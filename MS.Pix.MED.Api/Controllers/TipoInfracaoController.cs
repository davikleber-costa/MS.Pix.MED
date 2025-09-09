using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Api.Controllers;

[ApiController]
[Authorize]
[Route("v1/med/tipo-infracao")]
[Produces("application/json")]
public class TipoInfracaoController : MedControllerBase
{
    [HttpPost]
    [ProducesResponseType<TipoInfracao>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarTipoInfracaoAsync([FromBody] TipoInfracao tipoInfracao, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de criação usando MediatR ou serviço
        // Por enquanto retorna Created como placeholder
        return CreatedAtAction(nameof(ObterTipoInfracaoPorIdAsync), new { id = tipoInfracao.IdTipoInfracao }, tipoInfracao);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<TipoInfracao>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterTipoInfracaoPorIdAsync(int id, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna NotFound como placeholder
        return NotFound($"Tipo de infração com ID {id} não encontrado");
    }

    [HttpGet]
    [ProducesResponseType<IEnumerable<TipoInfracao>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterTiposInfracaoAtivosAsync(CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna lista vazia como placeholder
        return Ok(new List<TipoInfracao>());
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType<TipoInfracao>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AtualizarTipoInfracaoAsync(int id, [FromBody] TipoInfracao tipoInfracao, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de atualização usando MediatR ou serviço
        // Por enquanto retorna NotFound como placeholder
        return NotFound($"Tipo de infração com ID {id} não encontrado");
    }
}