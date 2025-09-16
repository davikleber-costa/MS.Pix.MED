using Microsoft.EntityFrameworkCore;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Data;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Infrastructure.Repository;

public class RetornoJdpiRepository : IRetornoJdpiRepository
{
    private readonly MedDbContext _context;
    private readonly DbSet<RetornoJdpi> _dbSet;

    public RetornoJdpiRepository(MedDbContext context)
    {
        _context = context;
        _dbSet = context.Set<RetornoJdpi>();
    }

    public async Task<RetornoJdpi> AddAsync(RetornoJdpi entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<RetornoJdpi> UpdateAsync(RetornoJdpi entity)
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

    public async Task<RetornoJdpi?> GetByIdAsync(long id)
    {
        return await _dbSet
            .Include(r => r.Transacao)
            .ThenInclude(t => t.TipoInfracao)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<IEnumerable<RetornoJdpi>> GetAllAsync()
    {
        return await _dbSet
            .Include(r => r.Transacao)
            .ThenInclude(t => t.TipoInfracao)
            .ToListAsync();
    }

    public async Task<IEnumerable<RetornoJdpi>> GetByTransacaoIdAsync(long transacaoId)
    {
        return await _dbSet
            .Include(r => r.Transacao)
            .ThenInclude(t => t.TipoInfracao)
            .Where(r => r.TransacaoId == transacaoId)
            .OrderBy(r => r.DataCriacao)
            .ThenBy(r => r.HoraCriacao)
            .ToListAsync();
    }

    public async Task<RetornoJdpi?> GetLatestByTransacaoIdAsync(long transacaoId)
    {
        return await _dbSet
            .Include(r => r.Transacao)
            .ThenInclude(t => t.TipoInfracao)
            .Where(r => r.TransacaoId == transacaoId)
            .OrderByDescending(r => r.DataCriacao)
            .ThenByDescending(r => r.HoraCriacao)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<RetornoJdpi>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _dbSet
            .Include(r => r.Transacao)
            .ThenInclude(t => t.TipoInfracao)
            .Where(r => r.DataCriacao >= startDate && r.DataCriacao <= endDate)
            .ToListAsync();
    }
}