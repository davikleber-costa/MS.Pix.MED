using MediatR;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.Transacao.Commands;

public record DeleteTransacaoCommand(int Id) : IRequest<bool>;

public class DeleteTransacaoCommandHandler : IRequestHandler<DeleteTransacaoCommand, bool>
{
    private readonly ITransacaoRepository _repository;

    public DeleteTransacaoCommandHandler(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<bool> Handle(DeleteTransacaoCommand request, CancellationToken cancellationToken)
    {
        var transacao = await _repository.GetByIdAsync(request.Id);
        if (transacao == null)
            return false;

        await _repository.DeleteAsync(transacao);
        return true;
    }
}
