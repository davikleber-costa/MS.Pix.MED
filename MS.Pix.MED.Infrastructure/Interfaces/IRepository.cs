using MS.Pix.Shared;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IRepository<T, TFilter, TUpdateDto>
    where T : class
    where TFilter : BaseFilter
{
    Task<IEnumerable<T>> GetListAsync(TFilter filter);
    Task<T?> GetAsync(long id);
    Task<T> SaveAsync(T entity);
    Task<T?> FindAsync(long id);
    Task<T> UpdateAsync(T entity, TUpdateDto updateDto);
}

public interface IRepository<T, TFilter>
    where T : class
    where TFilter : BaseFilter
{
    Task<IEnumerable<T>> GetAsync(TFilter filter);
    Task<IEnumerable<T>> GetListAsync(TFilter filter);
}