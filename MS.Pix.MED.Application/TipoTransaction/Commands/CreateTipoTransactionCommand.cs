using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoTransaction.Commands;

public record CreateTipoTransactionCommand(
    [Required] int IdTpTransaction,
    [Required(AllowEmptyStrings = false)] string DsDescricao,
    bool StAtivo = true
) : IRequest<CreateTipoTransactionResult>;

public record CreateTipoTransactionResult(
    int IdTpTransaction,
    string DsDescricao,
    bool StAtivo
);