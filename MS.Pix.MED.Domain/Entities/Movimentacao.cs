namespace MS.Pix.MED.Domain.Entities;

public class Movimentacao
{
    public int IdMovimentacao { get; set; }
    
    public string? IdRelatoInfracao { get; set; }
    
    public Guid IdExtrato { get; set; }
    
    public DateTime DtInclusao { get; set; }
    
    public int IdTipoInfracao { get; set; }
    
    public string? FileBase64 { get; set; }
    
    public int? IdLog { get; set; }
    
    // Navigation properties
    public virtual TipoInfracao TipoInfracao { get; set; } = null!;
    
    public virtual JdpiLog? JdpiLog { get; set; }
}