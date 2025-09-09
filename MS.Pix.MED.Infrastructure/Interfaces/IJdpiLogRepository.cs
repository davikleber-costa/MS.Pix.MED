using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IJdpiLogRepository : IRepository<JdpiLog, JdpiLogFilter, JdpiLogUpdateDto>
{
    Task<IEnumerable<JdpiLog>> GetByIdExtratoAsync(Guid idExtrato, CancellationToken cancellationToken = default);

    Task<IEnumerable<JdpiLog>> GetByTipoTransacaoAsync(int tipoTransacao, CancellationToken cancellationToken = default);

    Task<IEnumerable<JdpiLog>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default);
}