using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.TipoInfracao.Commands;

public record UpdateTipoInfracaoCommand(
    long Id,
    string Descricao,
    bool Ativo
) : IRequest<Domain.Entities.TipoInfracao?>;

public class UpdateTipoInfracaoCommandHandler : IRequestHandler<UpdateTipoInfracaoCommand, Domain.Entities.TipoInfracao?>
{
    private readonly ITipoInfracaoRepository _repository;

    public UpdateTipoInfracaoCommandHandler(ITipoInfracaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.TipoInfracao?> Handle(UpdateTipoInfracaoCommand request, CancellationToken cancellationToken)
    {
        var tipoInfracao = await _repository.GetByIdAsync(request.Id);
        
        if (tipoInfracao == null)
            return null;

        tipoInfracao.Descricao = request.Descricao;
        tipoInfracao.Ativo = request.Ativo;

        return await _repository.UpdateAsync(tipoInfracao);
    }
}
