using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.JdpiLog.Commands;

public record CreateJdpiLogCommand(
    [Required] DateTime DtLog,
    string? IdRelatoInfracao,
    short? StRelatoInfracao,
    [Required] Guid IdExtrato,
    string? ResponseJDPI,
    string? RequestJDPI,
    bool? TpTransaction
) : IRequest<CreateJdpiLogResult>;

public record CreateJdpiLogResult(
    int IdLog,
    DateTime DtLog,
    string? IdRelatoInfracao,
    short? StRelatoInfracao,
    Guid IdExtrato,
    string? ResponseJDPI,
    string? RequestJDPI,
    bool? TpTransaction
);