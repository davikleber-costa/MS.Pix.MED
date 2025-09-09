using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.JdpiLog.Commands;

public record UpdateJdpiLogCommand(
    [Required] int IdLog,
    [Required] DateTime DtLog,
    string? IdRelatoInfracao,
    short? StRelatoInfracao,
    [Required] Guid IdExtrato,
    string? ResponseJDPI,
    string? RequestJDPI,
    bool? TpTransaction
) : IRequest<UpdateJdpiLogResult>;

public record UpdateJdpiLogResult(
    int IdLog,
    DateTime DtLog,
    string? IdRelatoInfracao,
    short? StRelatoInfracao,
    Guid IdExtrato,
    string? ResponseJDPI,
    string? RequestJDPI,
    bool? TpTransaction
);