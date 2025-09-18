using MediatR;
using Microsoft.AspNetCore.Mvc;
using MS.Pix.MED.Application.TipoInfracao.Commands;
using MS.Pix.MED.Application.TipoInfracao.Queries;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Api.Controllers;

public class TipoInfracaoController : MedControllerBase
{
    private readonly IMediator _mediator;

    public TipoInfracaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TipoInfracao>> GetById(long id)
    {
        var result = await _mediator.Send(new GetTipoInfracaoByIdQuery(id));
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<TipoInfracao>>> GetActive()
    {
        var result = await _mediator.Send(new GetActiveTiposInfracaoQuery());
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<TipoInfracao>> Create([FromBody] CreateTipoInfracaoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TipoInfracao>> Update(long id, [FromBody] UpdateTipoInfracaoCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(long id)
    {
        var result = await _mediator.Send(new DeleteTipoInfracaoCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}