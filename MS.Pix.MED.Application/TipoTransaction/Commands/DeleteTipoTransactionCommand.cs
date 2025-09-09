using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoTransaction.Commands;

public record DeleteTipoTransactionCommand(
    [Required] int IdTpTransaction
) : IRequest<DeleteTipoTransactionResult>;

public record DeleteTipoTransactionResult(
    int IdTpTransaction,
    bool Success,
    string? Message = null
);