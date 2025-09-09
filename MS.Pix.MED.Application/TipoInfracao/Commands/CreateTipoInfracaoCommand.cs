using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoInfracao.Commands;

public record CreateTipoInfracaoCommand(
    [Required(AllowEmptyStrings = false)] string DsDescricao,
    bool StAtivo = true
) : IRequest<CreateTipoInfracaoResult>;

public record CreateTipoInfracaoResult(
    int IdTipoInfracao,
    string DsDescricao,
    bool StAtivo
);