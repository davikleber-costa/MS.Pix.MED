using Dapper;
using Microsoft.Extensions.Logging;
using MS.Pix.MED.Domain.Entities;
using MS.Pix.MED.Infrastructure.Interfaces;
using Npgsql;
using System.Data;

namespace MS.Pix.MED.Infrastructure.Repository;

public class JdpiLogRepository : IJdpiLogRepository
{
    private readonly string _connectionString;
    private readonly ILogger<JdpiLogRepository> _logger;

    public JdpiLogRepository(string connectionString, ILogger<JdpiLogRepository> logger)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    private IDbConnection CreateConnection()
    {
        return new NpgsqlConnection(_connectionString);
    }

    public async Task<PaginableEntity<JdpiLog>> GetListAsync(JdpiLogFilter filter, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        // TODO: Implementar consulta com filtros e paginação
        var sql = "SELECT * FROM TbMEDJdpiLog ORDER BY DtLog DESC LIMIT @Limit OFFSET @Offset";
        
        var logs = await connection.QueryAsync<JdpiLog>(sql, new { Limit = filter.PageSize, Offset = filter.Skip });
        var total = await connection.QuerySingleAsync<int>("SELECT COUNT(*) FROM TbMEDJdpiLog");
        
        return new PaginableEntity<JdpiLog>
        {
            Items = logs,
            TotalCount = total,
            PageSize = filter.PageSize,
            CurrentPage = filter.Page
        };
    }

    public async Task<JdpiLog?> GetAsync(object filters, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        // TODO: Implementar consulta específica baseada nos filtros
        var sql = "SELECT * FROM TbMEDJdpiLog WHERE IdLog = @IdLog";
        
        return await connection.QuerySingleOrDefaultAsync<JdpiLog>(sql, filters);
    }

    public async Task<Guid> SaveAsync(JdpiLog entity, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        var sql = @"
            INSERT INTO TbMEDJdpiLog (DtLog, IdRelatoInfracao, StRelatoInfracao, IdExtrato, ResponseJDPI, RequestJDPI, TpTransaction)
            VALUES (@DtLog, @IdRelatoInfracao, @StRelatoInfracao, @IdExtrato, @ResponseJDPI, @RequestJDPI, @TpTransaction)
            RETURNING IdLog";
        
        entity.DtLog = DateTime.UtcNow;
        
        var id = await connection.QuerySingleAsync<int>(sql, entity);
        
        _logger.LogInformation("Log JDPI criado com sucesso. ID: {Id}", id);
        
        return Guid.NewGuid(); // Retorna Guid temporário para compatibilidade
    }

    public async Task<JdpiLog?> FindAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        var sql = "SELECT * FROM TbMEDJdpiLog WHERE IdLog = @IdLog";
        
        // Nota: A entidade usa int como ID, mas a interface espera Guid
        // TODO: Revisar interface para usar int ou criar mapeamento
        return await connection.QuerySingleOrDefaultAsync<JdpiLog>(sql, new { IdLog = id });
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        var sql = "DELETE FROM TbMEDJdpiLog WHERE IdLog = @IdLog";
        
        await connection.ExecuteAsync(sql, new { IdLog = id });
        
        _logger.LogInformation("Log JDPI removido. ID: {Id}", id);
    }

    public async Task<Guid> UpdateAsync(JdpiLogUpdateDto updateData, JdpiLog entity, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        var sql = @"
            UPDATE TbMEDJdpiLog 
            SET ResponseJDPI = @ResponseJDPI, 
                TpTransaction = @TpTransaction,
                StRelatoInfracao = @StRelatoInfracao
            WHERE IdLog = @IdLog";
        
        await connection.ExecuteAsync(sql, new 
        { 
            ResponseJDPI = updateData.ResponseJDPI,
            TpTransaction = updateData.TpTransaction,
            StRelatoInfracao = updateData.ComErro.HasValue ? (updateData.ComErro.Value ? (short)1 : (short)0) : (short?)null,
            IdLog = entity.IdLog 
        });
        
        _logger.LogInformation("Log JDPI atualizado. ID: {Id}", entity.IdLog);
        
        return Guid.NewGuid(); // Retorna Guid temporário para compatibilidade
    }

    public async Task<IEnumerable<JdpiLog>> GetByIdExtratoAsync(Guid idExtrato, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        var sql = "SELECT * FROM TbMEDJdpiLog WHERE IdExtrato = @IdExtrato ORDER BY DtLog DESC";
        
        return await connection.QueryAsync<JdpiLog>(sql, new { IdExtrato = idExtrato });
    }

    public async Task<IEnumerable<JdpiLog>> GetByTipoTransacaoAsync(int tipoTransacao, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        var sql = "SELECT * FROM TbMEDJdpiLog WHERE TpTransaction = @TpTransaction ORDER BY DtLog DESC";
        
        // Converte int para bool (0 = false, 1 = true)
        var tpTransaction = tipoTransacao == 1;
        
        return await connection.QueryAsync<JdpiLog>(sql, new { TpTransaction = tpTransaction });
    }

    public async Task<IEnumerable<JdpiLog>> GetByPeriodoAsync(DateTime dataInicio, DateTime dataFim, CancellationToken cancellationToken = default)
    {
        using var connection = CreateConnection();
        
        var sql = @"
            SELECT * FROM TbMEDJdpiLog 
            WHERE DtLog >= @DataInicio 
              AND DtLog <= @DataFim 
            ORDER BY DtLog DESC";
        
        return await connection.QueryAsync<JdpiLog>(sql, new { DataInicio = dataInicio, DataFim = dataFim });
    }
}