using Microsoft.EntityFrameworkCore;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Data;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Infrastructure.Repository;

public class TipoInfracaoRepository : ITipoInfracaoRepository
{
    private readonly MedDbContext _context;
    private readonly DbSet<TipoInfracao> _dbSet;

    public TipoInfracaoRepository(MedDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TipoInfracao>();
    }

    public async Task<TipoInfracao> AddAsync(TipoInfracao entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<TipoInfracao> UpdateAsync(TipoInfracao entity)
    {
        _dbSet.Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task DeleteAsync(long id)
    {
        var entity = await GetByIdAsync(id);
        if (entity != null)
        {
            _dbSet.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<TipoInfracao?> GetByIdAsync(long id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<TipoInfracao>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<IEnumerable<TipoInfracao>> GetActiveTiposInfracaoAsync()
    {
        return await _dbSet.Where(t => t.Ativo).ToListAsync();
    }

    public async Task<TipoInfracao?> GetByDescricaoAsync(string descricao)
    {
        return await _dbSet.FirstOrDefaultAsync(t => t.Descricao == descricao);
    }
}