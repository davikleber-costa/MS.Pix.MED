using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IRetornoJdpiRepository
{
    Task<RetornoJdpi> AddAsync(RetornoJdpi entity);
    Task<RetornoJdpi> UpdateAsync(RetornoJdpi entity);
    Task DeleteAsync(long id);
    Task<RetornoJdpi?> GetByIdAsync(long id);
    Task<IEnumerable<RetornoJdpi>> GetAllAsync();
    Task<IEnumerable<RetornoJdpi>> GetByTransacaoIdAsync(long transacaoId);
    Task<RetornoJdpi?> GetLatestByTransacaoIdAsync(long transacaoId);
    Task<IEnumerable<RetornoJdpi>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
}