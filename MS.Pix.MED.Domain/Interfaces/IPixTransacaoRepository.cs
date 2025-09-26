using MS.Pix.MED.Domain.DTOs;

namespace MS.Pix.MED.Domain.Interfaces;

public interface IPixTransacaoRepository
{
    Task<PixTransacaoDto?> GetByComprovanteIdAsync(string comprovanteId);
}