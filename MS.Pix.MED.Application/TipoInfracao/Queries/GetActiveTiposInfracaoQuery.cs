using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.TipoInfracao.Queries;

public record GetActiveTiposInfracaoQuery : IRequest<IEnumerable<Domain.Entities.TipoInfracao>>;

public class GetActiveTiposInfracaoQueryHandler : IRequestHandler<GetActiveTiposInfracaoQuery, IEnumerable<Domain.Entities.TipoInfracao>>
{
    private readonly ITipoInfracaoRepository _repository;

    public GetActiveTiposInfracaoQueryHandler(ITipoInfracaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.TipoInfracao>> Handle(GetActiveTiposInfracaoQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetActiveTiposInfracaoAsync();
    }
}
