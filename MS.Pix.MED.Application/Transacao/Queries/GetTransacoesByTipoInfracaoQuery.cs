using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.Transacao.Queries;

public record GetTransacoesByTipoInfracaoQuery(int TipoInfracaoId) : IRequest<IEnumerable<Domain.Entities.Transacao>>;

public class GetTransacoesByTipoInfracaoQueryHandler : IRequestHandler<GetTransacoesByTipoInfracaoQuery, IEnumerable<Domain.Entities.Transacao>>
{
    private readonly ITransacaoRepository _repository;

    public GetTransacoesByTipoInfracaoQueryHandler(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.Transacao>> Handle(GetTransacoesByTipoInfracaoQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByTipoInfracaoIdAsync(request.TipoInfracaoId);
    }
}