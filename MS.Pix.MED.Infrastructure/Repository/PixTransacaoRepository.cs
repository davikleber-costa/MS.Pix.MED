using MS.Pix.MED.Domain.DTOs;
using MS.Pix.MED.Domain.Interfaces;

namespace MS.Pix.MED.Infrastructure.Repository;

public class PixTransacaoRepository : IPixTransacaoRepository
{
    private readonly IMySqlContext _mySqlContext;

    public PixTransacaoRepository(IMySqlContext mySqlContext)
    {
        _mySqlContext = mySqlContext;
    }

    public async Task<PixTransacaoDto?> GetByComprovanteIdAsync(string comprovanteId)
    {
        const string sql = @"
            SELECT 
                ispb_origem as IspbOrigem,
                ispb_destino as IspbDestino,
                id_end_to_end as EndToEndId,
                agencia,
                conta,
                valor as Valor,
                valor_detalhes as ValorDetalhes,
                criado_em as DataTransacao,
                status as Status
            FROM ibanking.pix_transacoes 
            WHERE id = @ComprovanteId
            ORDER BY criado_em DESC
            LIMIT 1";

        return await _mySqlContext.QueryFirstOrDefaultAsync<PixTransacaoDto>(sql, new { ComprovanteId = comprovanteId });
    }
}