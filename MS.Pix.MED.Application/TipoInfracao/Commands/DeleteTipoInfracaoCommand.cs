using MediatR;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.TipoInfracao.Commands;

public record DeleteTipoInfracaoCommand(int Id) : IRequest<bool>;

public class DeleteTipoInfracaoCommandHandler : IRequestHandler<DeleteTipoInfracaoCommand, bool>
{
    private readonly ITipoInfracaoRepository _repository;

    public DeleteTipoInfracaoCommandHandler(ITipoInfracaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTipoInfracaoCommand request, CancellationToken cancellationToken)
    {
        var tipoInfracao = await _repository.GetByIdAsync(request.Id);
        if (tipoInfracao == null)
            return false;

        await _repository.DeleteAsync(tipoInfracao);
        return true;
    }
}
