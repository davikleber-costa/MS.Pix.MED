using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoInfracao.Queries;

public record GetTipoInfracaoByIdQuery(
    [Required] int IdTipoInfracao,
    bool IncluirMovimentacoes = false
) : IRequest<GetTipoInfracaoByIdResult>;

public record GetTipoInfracaoByIdResult(
    Domain.Entities.TipoInfracao? TipoInfracao,
    bool Found,
    string? Message = null
);