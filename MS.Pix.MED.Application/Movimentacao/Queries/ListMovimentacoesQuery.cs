using MediatR;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Application.Movimentacao.Queries;

public record ListMovimentacoesQuery(
    DateTime? DataInicio = null,
    DateTime? DataFim = null,
    string? IdRelatoInfracao = null,
    Guid? IdExtrato = null,
    int? IdTipoInfracao = null,
    int? IdLog = null,
    bool IncluirTipoInfracao = false,
    bool IncluirJdpiLog = false,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ListMovimentacoesResult>;

public record ListMovimentacoesResult(
    IEnumerable<Domain.Entities.Movimentacao> Movimentacoes,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);