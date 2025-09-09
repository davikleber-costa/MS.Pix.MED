using MS.Pix.MED.Domain.Entities;

namespace MS.Pix.MED.Infrastructure.Interfaces;

public interface IRepository<T, TFilter, TUpdateDto> where T : BaseEntity
{
    Task<PaginableEntity<T>> GetListAsync(TFilter filter, CancellationToken cancellationToken = default);
    Task<T?> GetAsync(object filters, CancellationToken cancellationToken = default);
    Task<Guid> SaveAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> FindAsync(Guid id, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Guid> UpdateAsync(TUpdateDto updateData, T entity, CancellationToken cancellationToken = default);
}

public interface IRepository<T, TFilter> where T : BaseEntity
{
    Task<T?> GetAsync(TFilter filter, CancellationToken cancellationToken = default);
    Task<PaginableEntity<T>> GetListAsync(TFilter filter, CancellationToken cancellationToken = default);
}