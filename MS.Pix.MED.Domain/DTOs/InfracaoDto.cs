namespace MS.Pix.MED.Domain.DTOs;

public sealed class IncluirRelatoInfracaoRequest
{
    public int Ispb { get; init; }
    public string EndToEndId { get; init; } = default!;
    public int Motivo { get; init; }
    public int TpSitOrigem { get; set; }
    public string? Detalhes { get; init; }
    public ContatoCriadorDto ContatoCriador { get; init; } = new();
}

public sealed class ContatoCriadorDto
{
    public string Email { get; set; } = default!;
    public string Telefone { get; set; } = default!;
}

public sealed class CancelarRelatoInfracaoRequest
{
    public int Ispb { get; init; }
    public string IdRelatoInfracao { get; init; } = default!;
}

public sealed class ListarRelatosInfracaoQuery
{
    public int Ispb { get; init; }
    public bool EhRelatoSolicitado { get; init; } = false;
    public int? StRelatoInfracao { get; init; } = null;
    public bool IncluiDetalhes { get; init; } = false;
    public DateTimeOffset? DtHrModificacaoInicio { get; init; }
    public DateTimeOffset? DtHrModificacaoFim { get; init; }
    public int Pagina { get; init; } = 1;
    public int TamanhoPagina { get; init; } = 50;
}