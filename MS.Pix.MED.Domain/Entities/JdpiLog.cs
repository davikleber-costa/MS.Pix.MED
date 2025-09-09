namespace MS.Pix.MED.Domain.Entities;

public class JdpiLog
{
    public int IdLog { get; set; }
    
    public DateTime DtLog { get; set; }
    
    public string? IdRelatoInfracao { get; set; }
    
    public short? StRelatoInfracao { get; set; }
    
    public Guid IdExtrato { get; set; }
    
    public string? ResponseJDPI { get; set; }
    
    public string? RequestJDPI { get; set; }
    
    public bool? TpTransaction { get; set; }
    
    // Navigation properties
    public virtual ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
}