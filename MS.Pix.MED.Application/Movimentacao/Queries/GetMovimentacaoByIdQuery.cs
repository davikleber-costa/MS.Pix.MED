using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.Movimentacao.Queries;

public record GetMovimentacaoByIdQuery(
    [Required] int IdMovimentacao,
    bool IncluirTipoInfracao = false,
    bool IncluirJdpiLog = false
) : IRequest<GetMovimentacaoByIdResult>;

public record GetMovimentacaoByIdResult(
    Domain.Entities.Movimentacao? Movimentacao,
    bool Found,
    string? Message = null
);