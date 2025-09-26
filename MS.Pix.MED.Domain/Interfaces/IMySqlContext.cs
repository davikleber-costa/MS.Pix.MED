namespace MS.Pix.MED.Domain.Interfaces;

public interface IMySqlContext
{
    Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null);
    Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null);
}