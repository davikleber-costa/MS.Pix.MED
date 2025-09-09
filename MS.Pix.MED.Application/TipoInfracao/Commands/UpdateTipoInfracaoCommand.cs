using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoInfracao.Commands;

public record UpdateTipoInfracaoCommand(
    [Required] int IdTipoInfracao,
    [Required(AllowEmptyStrings = false)] string DsDescricao,
    bool StAtivo
) : IRequest<UpdateTipoInfracaoResult>;

public record UpdateTipoInfracaoResult(
    int IdTipoInfracao,
    string DsDescricao,
    bool StAtivo
);