using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.Movimentacao.Commands;

public record DeleteMovimentacaoCommand(
    [Required] int IdMovimentacao
) : IRequest<DeleteMovimentacaoResult>;

public record DeleteMovimentacaoResult(
    int IdMovimentacao,
    bool Success,
    string? Message = null
);