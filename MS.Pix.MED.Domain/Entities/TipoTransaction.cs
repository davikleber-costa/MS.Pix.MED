namespace MS.Pix.MED.Domain.Entities;

public class TipoTransaction
{
    public int IdTpTransaction { get; set; }
    
    public string DsDescricao { get; set; } = string.Empty;
    
    public bool StAtivo { get; set; } = true;
    
    // Navigation properties
    public virtual ICollection<JdpiLog> JdpiLogs { get; set; } = new List<JdpiLog>();
}