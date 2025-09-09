using MediatR;
using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Application.TipoInfracao.Queries;

public record ListTipoInfracoesQuery(
    bool? SomenteAtivos = null,
    string? DescricaoContains = null,
    bool IncluirMovimentacoes = false,
    int PageNumber = 1,
    int PageSize = 10
) : IRequest<ListTipoInfracoesResult>;

public record ListTipoInfracoesResult(
    IEnumerable<Domain.Entities.TipoInfracao> TiposInfracao,
    int TotalCount,
    int PageNumber,
    int PageSize,
    int TotalPages
);