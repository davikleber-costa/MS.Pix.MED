namespace MS.Pix.MED.Domain.DTOs;

public sealed class ContestacaoMedDto
{
    public string ComprovanteId { get; set; } = default!;
    public int TpSitOrigem { get; set; }
    public string? Detalhes { get; set; }
}