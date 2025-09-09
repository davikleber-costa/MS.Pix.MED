namespace MS.Pix.MED.Infrastructure.Repository;

public class JdpiLogUpdateDto
{
    public string? ResponseJDPI { get; set; }

    public string? TpTransaction { get; set; }

    public bool? ComErro { get; set; }

    public string? MensagemErro { get; set; }

    public int? StatusCode { get; set; }

    public long? TempoResposta { get; set; }
}