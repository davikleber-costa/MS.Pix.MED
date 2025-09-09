namespace MS.Pix.MED.Domain.Entities;

public class TipoInfracao
{
    public int IdTipoInfracao { get; set; }
    public string DsDescricao { get; set; } = string.Empty;
    public bool StAtivo { get; set; }
    
    // Navigation property
    public virtual ICollection<Movimentacao> Movimentacoes { get; set; } = new List<Movimentacao>();
}