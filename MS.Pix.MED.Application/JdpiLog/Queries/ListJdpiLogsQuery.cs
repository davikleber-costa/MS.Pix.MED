using MediatR;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Application.JdpiLog.Queries;

public record ListJdpiLogsQuery(
    DateTime? DataInicio = null,
    DateTime? DataFim = null,
    string? IdRelatoInfracao = null,
    short? StRelatoInfracao = null,
    Guid? IdExtrato = null,
    bool? TpTransaction = null,
    bool IncluirMovimentacoes = false,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ListJdpiLogsResult>;

public record ListJdpiLogsResult(
    IEnumerable<Domain.Entities.JdpiLog> Logs,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);