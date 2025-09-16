namespace MS.Pix.MED.Domain.Entities;

public class TipoInfracao
{
    public long Id { get; set; }
    public string Descricao { get; set; } = string.Empty;
    public bool Ativo { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Transacao> Transacoes { get; set; } = new List<Transacao>();
}