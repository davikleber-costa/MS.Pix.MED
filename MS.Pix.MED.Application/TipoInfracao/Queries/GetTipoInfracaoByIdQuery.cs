using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.TipoInfracao.Queries;

public record GetTipoInfracaoByIdQuery(int Id) : IRequest<Domain.Entities.TipoInfracao?>;

public class GetTipoInfracaoByIdQueryHandler : IRequestHandler<GetTipoInfracaoByIdQuery, Domain.Entities.TipoInfracao?>
{
    private readonly ITipoInfracaoRepository _repository;

    public GetTipoInfracaoByIdQueryHandler(ITipoInfracaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.TipoInfracao?> Handle(GetTipoInfracaoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}