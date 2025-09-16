using MediatR;
using Microsoft.AspNetCore.Mvc;
using MS.Pix.MED.Application.RetornoJdpi.Commands;
using MS.Pix.MED.Application.RetornoJdpi.Queries;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Api.Controllers;

public class RetornoJdpiController : MedControllerBase
{
    private readonly IMediator _mediator;

    public RetornoJdpiController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("by-transacao/{transacaoId}")]
    public async Task<ActionResult<IEnumerable<RetornoJdpi>>> GetByTransacao(long transacaoId)
    {
        var result = await _mediator.Send(new GetRetornoJdpiByTransacaoQuery(transacaoId));
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<RetornoJdpi>> Create([FromBody] CreateRetornoJdpiCommand command)
    {
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetByTransacao), new { transacaoId = result.TransacaoId }, result);
    }
}