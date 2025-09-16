using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.Transacao.Commands;

public record CreateTransacaoCommand(
    int TipoInfracaoId,
    string IdNotificacaoJdpi,
    string StatusRelatoJdpi,
    string? GuidExtratoJdpi = null,
    string? CaminhoArquivo = null
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
            GuidExtratoJdpi = request.GuidExtratoJdpi,
            CaminhoArquivo = request.CaminhoArquivo,
            DataCriacao = DateOnly.FromDateTime(DateTime.Now),
            HoraCriacao = TimeOnly.FromDateTime(DateTime.Now)
        };

        return await _repository.AddAsync(transacao);
    }
}