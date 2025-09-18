using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface ITransacaoRepository
{
    Task<Transacao> AddAsync(Transacao entity);
    Task<Transacao> UpdateAsync(Transacao entity);
    Task DeleteAsync(long id);
    Task<Transacao?> GetByIdAsync(long id);
    Task<IEnumerable<Transacao>> GetAllAsync();
    Task<IEnumerable<Transacao>> GetByTipoInfracaoIdAsync(long tipoInfracaoId);
    Task<IEnumerable<Transacao>> GetTransacoesByTipoInfracaoAsync(Guid tipoInfracaoId);
    Task<Transacao?> GetByIdNotificacaoJdpiAsync(string idNotificacaoJdpi);
    Task<IEnumerable<Transacao>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transacao>> GetWithRetornosAsync();
}