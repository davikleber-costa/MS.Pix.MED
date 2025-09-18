using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.Transacao.Commands;

public record CreateTransacaoCommand(
    long TipoInfracaoId,
    string IdNotificacaoJdpi,
    bool StatusRelatoJdpi,
    string? GuidExtratoJdpi = null,
    string? CaminhoArquivo = null,
    string? Agencia = null,
    string? Conta = null,
    string? Observacao = null
) : IRequest<Domain.Entities.Transacao>;

public class CreateTransacaoCommandHandler : IRequestHandler<CreateTransacaoCommand, Domain.Entities.Transacao>
{
    private readonly ITransacaoRepository _repository;

    public CreateTransacaoCommandHandler(ITransacaoRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.Transacao> Handle(CreateTransacaoCommand request, CancellationToken cancellationToken)
    {
        var transacao = new Domain.Entities.Transacao
        {
            TipoInfracaoId = request.TipoInfracaoId,
            IdNotificacaoJdpi = request.IdNotificacaoJdpi,
            StatusRelatoJdpi = request.StatusRelatoJdpi,
            GuidExtratoJdpi = request.GuidExtratoJdpi ?? string.Empty,
            CaminhoArquivo = request.CaminhoArquivo ?? string.Empty,
            Agencia = request.Agencia,
            Conta = request.Conta,
            Observacao = request.Observacao,
            DataCriacao = DateTime.Now,
            HoraCriacao = TimeOnly.FromDateTime(DateTime.Now)
        };

        return await _repository.AddAsync(transacao);
    }
}