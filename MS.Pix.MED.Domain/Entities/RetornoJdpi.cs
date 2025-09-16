namespace MS.Pix.MED.Domain.Entities;

public class RetornoJdpi
{
    public long Id { get; set; }
    public long TransacaoId { get; set; }
    public string? RequisicaoJdpi { get; set; }
    public string? RespostaJdpi { get; set; }
    public DateTime DataCriacao { get; set; }
    public TimeSpan HoraCriacao { get; set; }

    // Navigation properties
    public virtual Transacao Transacao { get; set; } = null!;
}