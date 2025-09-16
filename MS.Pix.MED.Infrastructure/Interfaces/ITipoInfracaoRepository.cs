using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface ITipoInfracaoRepository : IRepository<TipoInfracao>
{
    Task<IEnumerable<TipoInfracao>> GetActiveTiposInfracaoAsync();
    Task<TipoInfracao?> GetByDescricaoAsync(string descricao);
}