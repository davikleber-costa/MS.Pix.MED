using MediatR;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Application.RetornoJdpi.Commands;

public record CreateRetornoJdpiCommand(
    long TransacaoId,
    string RequisicaoJdpi,
    string RespostaJdpi
) : IRequest<Domain.Entities.RetornoJdpi>;

public class CreateRetornoJdpiCommandHandler : IRequestHandler<CreateRetornoJdpiCommand, Domain.Entities.RetornoJdpi>
{
    private readonly IRetornoJdpiRepository _repository;

    public CreateRetornoJdpiCommandHandler(IRetornoJdpiRepository repository)
    {
        _repository = repository;
    }

    public async Task<Domain.Entities.RetornoJdpi> Handle(CreateRetornoJdpiCommand request, CancellationToken cancellationToken)
    {
        var retornoJdpi = new Domain.Entities.RetornoJdpi
        {
            TransacaoId = request.TransacaoId,
            RequisicaoJdpi = request.RequisicaoJdpi,
            RespostaJdpi = request.RespostaJdpi,
            DataCriacao = DateOnly.FromDateTime(DateTime.Now),
            HoraCriacao = TimeOnly.FromDateTime(DateTime.Now)
        };

        return await _repository.AddAsync(retornoJdpi);
    }
}