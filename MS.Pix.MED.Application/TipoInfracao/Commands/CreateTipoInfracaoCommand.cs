using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.TipoInfracao.Commands;

public record CreateTipoInfracaoCommand(
    string Descricao,
    bool Ativo = true
) : IRequest<Domain.Entities.TipoInfracao>;

public class CreateTipoInfracaoCommandHandler : IRequestHandler<CreateTipoInfracaoCommand, Domain.Entities.TipoInfracao>
{
    private readonly ITipoInfracaoRepository _repository;

    public CreateTipoInfracaoCommandHandler(ITipoInfracaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.TipoInfracao> Handle(CreateTipoInfracaoCommand request, CancellationToken cancellationToken)
    {
        var tipoInfracao = new Domain.Entities.TipoInfracao
        {
            Descricao = request.Descricao,
            Ativo = request.Ativo
        };

        return await _repository.AddAsync(tipoInfracao);
    }
}
