using MS.Pix.Shared;

namespace MS.Pix.MED.Infrastructure.Repository;

public class JdpiLogFilter : BaseFilter
{
    public Guid? IdExtrato { get; set; }

    public int? TipoTransacao { get; set; }

    public DateTime? DataInicio { get; set; }

    public DateTime? DataFim { get; set; }

    public string? TpTransaction { get; set; }

    public bool? ComErro { get; set; }

    public JdpiLogFilter()
    {
        // Valores padrão para paginação
        Page = 1;
        PageSize = 50;
    }
}