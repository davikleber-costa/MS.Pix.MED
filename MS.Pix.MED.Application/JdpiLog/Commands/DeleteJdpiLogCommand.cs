using System.ComponentModel.DataAnnotations;
using MediatR;

namespace MS.Pix.MED.Application.JdpiLog.Commands;

public record DeleteJdpiLogCommand(
    [Required] int IdLog
) : IRequest<DeleteJdpiLogResult>;

public record DeleteJdpiLogResult(
    int IdLog,
    bool Success,
    string? Message = null
);