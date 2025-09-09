using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.JdpiLog.Queries;

public record GetJdpiLogByIdQuery(
    [Required] int IdLog,
    bool IncluirMovimentacoes = false
) : IRequest<GetJdpiLogByIdResult>;

public record GetJdpiLogByIdResult(
    Domain.Entities.JdpiLog? Log,
    bool Found,
    string? Message = null
);