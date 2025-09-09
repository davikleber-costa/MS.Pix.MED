using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoTransaction.Commands;

public record UpdateTipoTransactionCommand(
    [Required] int IdTpTransaction,
    [Required(AllowEmptyStrings = false)] string DsDescricao,
    bool StAtivo
) : IRequest<UpdateTipoTransactionResult>;

public record UpdateTipoTransactionResult(
    int IdTpTransaction,
    string DsDescricao,
    bool StAtivo
);