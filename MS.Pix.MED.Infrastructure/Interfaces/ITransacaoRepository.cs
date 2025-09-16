using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface ITransacaoRepository : IRepository<Transacao>
{
    Task<IEnumerable<Transacao>> GetByTipoInfracaoIdAsync(long tipoInfracaoId);
    Task<Transacao?> GetByIdNotificacaoJdpiAsync(string idNotificacaoJdpi);
    Task<IEnumerable<Transacao>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transacao>> GetWithRetornosAsync();
}