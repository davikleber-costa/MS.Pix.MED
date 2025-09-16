namespace MS.Pix.MED.Domain.Entities;

public class Transacao
{
    public long Id { get; set; }
    public long TipoInfracaoId { get; set; }
    public string IdNotificacaoJdpi { get; set; } = string.Empty;
    public bool StatusRelatoJdpi { get; set; }
    public string GuidExtratoJdpi { get; set; } = string.Empty;
    public string CaminhoArquivo { get; set; } = string.Empty;
    public DateTime DataCriacao { get; set; }
    public TimeOnly HoraCriacao { get; set; }

    // Navigation properties
    public virtual TipoInfracao TipoInfracao { get; set; } = null!;
    public virtual ICollection<RetornoJdpi> RetornosJdpi { get; set; } = new List<RetornoJdpi>();
}