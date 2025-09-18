using MS.Pix.MED.Domain.Entities;
using MS.Pix.Shared;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface ITransacaoRepository : IRepository<Transacao, BaseFilter, object>
{
    Task<IEnumerable<Transacao>> GetTransacoesByTipoInfracaoAsync(Guid tipoInfracaoId);
    Task<Transacao?> GetByIdNotificacaoJdpiAsync(string idNotificacaoJdpi);
    Task<IEnumerable<Transacao>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<Transacao>> GetWithRetornosAsync();
}