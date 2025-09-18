using Microsoft.EntityFrameworkCore;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Data;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Infrastructure.Repository;

public class TransacaoRepository : ITransacaoRepository
{
    private readonly MedDbContext _context;
    private readonly DbSet<Transacao> _dbSet;

    public TransacaoRepository(MedDbContext context)
    {
        _context = context;
        _dbSet = context.Set<Transacao>();
    }

    public async Task<Transacao> AddAsync(Transacao entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<Transacao> UpdateAsync(Transacao entity)
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

    public async Task<Transacao?> GetByIdAsync(long id)
    {
        return await _dbSet
            .Include(t => t.TipoInfracao)
            .Include(t => t.RetornosJdpi)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<IEnumerable<Transacao>> GetAllAsync()
    {
        return await _dbSet
            .Include(t => t.TipoInfracao)
            .Include(t => t.RetornosJdpi)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transacao>> GetByTipoInfracaoIdAsync(long tipoInfracaoId)
    {
        return await _dbSet
            .Include(t => t.TipoInfracao)
            .Include(t => t.RetornosJdpi)
            .Where(t => t.TipoInfracaoId == tipoInfracaoId)
            .ToListAsync();
    }

    public async Task<Transacao?> GetByIdNotificacaoJdpiAsync(string idNotificacaoJdpi)
    {
        return await _dbSet
            .Include(t => t.TipoInfracao)
            .Include(t => t.RetornosJdpi)
            .FirstOrDefaultAsync(t => t.IdNotificacaoJdpi == idNotificacaoJdpi);
    }

    public async Task<IEnumerable<Transacao>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(t => t.TipoInfracao)
            .Include(t => t.RetornosJdpi)
            .Where(t => t.DataCriacao >= startDate && t.DataCriacao <= endDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<Transacao>> GetWithRetornosAsync()
    {
        return await _dbSet
            .Include(t => t.TipoInfracao)
            .Include(t => t.RetornosJdpi)
            .Where(t => t.RetornosJdpi.Any())
            .ToListAsync();
    }

    public async Task<IEnumerable<Transacao>> GetTransacoesByTipoInfracaoAsync(Guid tipoInfracaoId)
    {
        return await _dbSet.Where(t => t.TipoInfracaoId == (long)tipoInfracaoId.GetHashCode()).ToListAsync();
    }
}