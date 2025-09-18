using MS.Pix.MED.Domain.Entities;
using MS.Pix.Shared;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface ITipoInfracaoRepository : IRepository<TipoInfracao, BaseFilter, object>
{
    Task<IEnumerable<TipoInfracao>> GetActiveTiposInfracaoAsync();
    Task<TipoInfracao?> GetByDescricaoAsync(string descricao);
}