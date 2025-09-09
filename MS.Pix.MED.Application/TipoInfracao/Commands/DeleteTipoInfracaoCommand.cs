using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.TipoInfracao.Commands;

public record DeleteTipoInfracaoCommand(
    [Required] int IdTipoInfracao
) : IRequest<DeleteTipoInfracaoResult>;

public record DeleteTipoInfracaoResult(
    int IdTipoInfracao,
    bool Success,
    string? Message = null
);