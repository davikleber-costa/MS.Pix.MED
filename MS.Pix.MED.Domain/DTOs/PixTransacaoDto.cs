namespace MS.Pix.MED.Domain.DTOs;

public sealed class PixTransacaoDto
{
    public int IspbOrigem { get; set; }
    public int IspbDestino { get; set; }
    public string EndToEndId { get; set; } = default!;
    public string? Agencia { get; set; }
    public string? Conta { get; set; }
    public decimal Valor { get; set; }
    public string? ValorDetalhes { get; set; }
    public DateTime DataTransacao { get; set; }
    public string? Status { get; set; }
}