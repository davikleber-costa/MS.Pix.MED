using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.Movimentacao.Commands;

public record CreateMovimentacaoCommand(
    string? IdRelatoInfracao,
    [Required] Guid IdExtrato,
    [Required] DateTime DtInclusao,
    [Required] int IdTipoInfracao,
    string? FileBase64,
    int? IdLog
) : IRequest<CreateMovimentacaoResult>;

public record CreateMovimentacaoResult(
    int IdMovimentacao,
    string? IdRelatoInfracao,
    Guid IdExtrato,
    DateTime DtInclusao,
    int IdTipoInfracao,
    string? FileBase64,
    int? IdLog
);