using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Api.Controllers;

[ApiController]
[Authorize]
[Route("v1/med/jdpi-log")]
[Produces("application/json")]
public class JdpiLogController : MedControllerBase
{
    [HttpPost]
    [ProducesResponseType<JdpiLog>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CriarJdpiLogAsync([FromBody] JdpiLog jdpiLog, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de criação usando MediatR ou serviço
        // Por enquanto retorna Created como placeholder
        return CreatedAtAction(nameof(ObterJdpiLogPorIdAsync), new { id = jdpiLog.IdLog }, jdpiLog);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType<JdpiLog>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status404NotFound)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterJdpiLogPorIdAsync(int id, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna NotFound como placeholder
        return NotFound($"Log JDPI com ID {id} não encontrado");
    }

    [HttpGet("extrato/{idExtrato:guid}")]
    [ProducesResponseType<IEnumerable<JdpiLog>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> ObterJdpiLogsPorExtratoAsync(Guid idExtrato, CancellationToken cancellationToken)
    {
        // TODO: Implementar lógica de busca usando MediatR ou serviço
        // Por enquanto retorna lista vazia como placeholder
        return Ok(new List<JdpiLog>());
    }
}