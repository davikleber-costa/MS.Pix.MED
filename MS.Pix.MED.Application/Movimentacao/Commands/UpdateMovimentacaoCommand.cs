using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.Movimentacao.Commands;

public record UpdateMovimentacaoCommand(
    [Required] int IdMovimentacao,
    string? IdRelatoInfracao,
    [Required] Guid IdExtrato,
    [Required] DateTime DtInclusao,
    [Required] int IdTipoInfracao,
    string? FileBase64,
    int? IdLog
) : IRequest<UpdateMovimentacaoResult>;

public record UpdateMovimentacaoResult(
    int IdMovimentacao,
    string? IdRelatoInfracao,
    Guid IdExtrato,
    DateTime DtInclusao,
    int IdTipoInfracao,
    string? FileBase64,
    int? IdLog
);