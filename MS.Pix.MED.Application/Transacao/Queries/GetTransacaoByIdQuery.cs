using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.Transacao.Queries;

public record GetTransacaoByIdQuery(int Id) : IRequest<Domain.Entities.Transacao?>;

public class GetTransacaoByIdQueryHandler : IRequestHandler<GetTransacaoByIdQuery, Domain.Entities.Transacao?>
{
    private readonly ITransacaoRepository _repository;

    public GetTransacaoByIdQueryHandler(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Transacao?> Handle(GetTransacaoByIdQuery request, CancellationToken cancellationToken)
    {
        return await _repository.GetByIdAsync(request.Id);
    }
}
