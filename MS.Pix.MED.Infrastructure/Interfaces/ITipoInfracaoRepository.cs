using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface ITipoInfracaoRepository
{
    Task<TipoInfracao> AddAsync(TipoInfracao entity);
    Task<TipoInfracao> UpdateAsync(TipoInfracao entity);
    Task DeleteAsync(long id);
    Task<TipoInfracao?> GetByIdAsync(long id);
    Task<IEnumerable<TipoInfracao>> GetAllAsync();
    Task<IEnumerable<TipoInfracao>> GetActiveTiposInfracaoAsync();
    Task<TipoInfracao?> GetByDescricaoAsync(string descricao);
}