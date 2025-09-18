using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.RetornoJdpi.Queries;

public record GetRetornoJdpiByTransacaoQuery(long TransacaoId) : IRequest<IEnumerable<Domain.Entities.RetornoJdpi>>;

public class GetRetornoJdpiByTransacaoQueryHandler : IRequestHandler<GetRetornoJdpiByTransacaoQuery, IEnumerable<Domain.Entities.RetornoJdpi>>
{
    private readonly IRetornoJdpiRepository _repository;

    public GetRetornoJdpiByTransacaoQueryHandler(IRetornoJdpiRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Domain.Entities.RetornoJdpi>> Handle(GetRetornoJdpiByTransacaoQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByTransacaoIdAsync(request.TransacaoId);
    }
}
