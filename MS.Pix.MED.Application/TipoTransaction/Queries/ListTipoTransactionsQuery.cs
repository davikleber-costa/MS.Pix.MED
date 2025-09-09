using MediatR;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Application.TipoTransaction.Queries;

public record ListTipoTransactionsQuery(
    bool? SomenteAtivos = null,
    string? DescricaoContains = null,
    bool IncluirJdpiLogs = false,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ListTipoTransactionsResult>;

public record ListTipoTransactionsResult(
    IEnumerable<Domain.Entities.TipoTransaction> TiposTransaction,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);