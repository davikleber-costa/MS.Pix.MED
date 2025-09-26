using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.Transacao.Commands;

public record UpdateTransacaoCommand(
    long Id,
    long TipoInfracaoId,
    string IdNotificacaoJdpi,
    bool StatusRelatoJdpi,
    string? GuidExtratoJdpi = null,
    string? CaminhoArquivo = null
    // string? Agencia = null,
    // string? Conta = null,
    // string? Observacao = null
) : IRequest<Domain.Entities.Transacao?>;

public class UpdateTransacaoCommandHandler : IRequestHandler<UpdateTransacaoCommand, Domain.Entities.Transacao?>
{
    private readonly ITransacaoRepository _repository;

    public UpdateTransacaoCommandHandler(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Transacao?> Handle(UpdateTransacaoCommand request, CancellationToken cancellationToken)
    {
        var transacao = await _repository.GetByIdAsync(request.Id);
        if (transacao == null)
            return null;

        transacao.TipoInfracaoId = request.TipoInfracaoId;
        transacao.IdNotificacaoJdpi = request.IdNotificacaoJdpi;
        transacao.StatusRelatoJdpi = request.StatusRelatoJdpi;
        transacao.GuidExtratoJdpi = request.GuidExtratoJdpi ?? string.Empty;
        transacao.CaminhoArquivo = request.CaminhoArquivo ?? string.Empty;
        // transacao.Agencia = request.Agencia;
        // transacao.Conta = request.Conta;
        // transacao.Observacao = request.Observacao;

        return await _repository.UpdateAsync(transacao);
    }
}