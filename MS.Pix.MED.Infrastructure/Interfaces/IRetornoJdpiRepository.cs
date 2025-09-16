using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IRetornoJdpiRepository : IRepository<RetornoJdpi>
{
    Task<IEnumerable<RetornoJdpi>> GetByTransacaoIdAsync(long transacaoId);
    Task<RetornoJdpi?> GetLatestByTransacaoIdAsync(long transacaoId);
    Task<IEnumerable<RetornoJdpi>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}