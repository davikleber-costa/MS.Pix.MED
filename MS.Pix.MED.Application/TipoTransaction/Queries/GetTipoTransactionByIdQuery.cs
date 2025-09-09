using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoTransaction.Queries;

public record GetTipoTransactionByIdQuery(
    [Required] int IdTpTransaction,
    bool IncluirJdpiLogs = false
) : IRequest<GetTipoTransactionByIdResult>;

public record GetTipoTransactionByIdResult(
    Domain.Entities.TipoTransaction? TipoTransaction,
    bool Found,
    string? Message = null
);