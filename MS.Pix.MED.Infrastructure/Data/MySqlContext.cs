using Dapper;
using Microsoft.Extensions.Configuration;
using MS.Pix.MED.Domain.Interfaces;
using MySql.Data.MySqlClient;

namespace MS.Pix.MED.Infrastructure.Data;

public class MySqlContext : IMySqlContext
{
    private readonly string _connectionString;

    public MySqlContext(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("MySqlConnection") 
            ?? throw new InvalidOperationException("MySqlConnection string not found");
    }

    public async Task<T?> QueryFirstOrDefaultAsync<T>(string sql, object? param = null)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryFirstOrDefaultAsync<T>(sql, param);
    }

    public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object? param = null)
    {
        using var connection = new MySqlConnection(_connectionString);
        await connection.OpenAsync();
        return await connection.QueryAsync<T>(sql, param);
    }
}