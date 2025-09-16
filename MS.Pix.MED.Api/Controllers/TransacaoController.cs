using MediatR;
using Microsoft.AspNetCore.Mvc;
using MS.Pix.MED.Application.Transacao.Commands;
using MS.Pix.MED.Application.Transacao.Queries;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Api.Controllers;

public class TransacaoController : MedControllerBase
{
    private readonly IMediator _mediator;

    public TransacaoController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Transacao>> GetById(int id)
    {
        var result = await _mediator.Send(new GetTransacaoByIdQuery(id));
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpGet("by-tipo-infracao/{tipoInfracaoId}")]
    public async Task<ActionResult<IEnumerable<Transacao>>> GetByTipoInfracao(int tipoInfracaoId)
    {
        var result = await _mediator.Send(new GetTransacoesByTipoInfracaoQuery(tipoInfracaoId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<Transacao>> Create([FromBody] CreateTransacaoCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Transacao>> Update(int id, [FromBody] UpdateTransacaoCommand command)
    {
        if (id != command.Id)
            return BadRequest("ID mismatch");

        var result = await _mediator.Send(command);
        if (result == null)
            return NotFound();

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var result = await _mediator.Send(new DeleteTransacaoCommand(id));
        if (!result)
            return NotFound();

        return NoContent();
    }
}