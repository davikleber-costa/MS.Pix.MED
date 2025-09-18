using MS.Pix.MED.Domain.Entities;
using MS.Pix.Shared;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IRetornoJdpiRepository : IRepository<RetornoJdpi, BaseFilter, object>
{
    Task<IEnumerable<RetornoJdpi>> GetByTransacaoIdAsync(long transacaoId);
    Task<RetornoJdpi?> GetLatestByTransacaoIdAsync(long transacaoId);
    Task<IEnumerable<RetornoJdpi>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}