using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using MS.Pix.MED.Infrastructure.Interfaces;

namespace MS.Pix.MED.Infrastructure.Middlewares;

public class RequestContextMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestContextMiddleware> _logger;

    public RequestContextMiddleware(RequestDelegate next, ILogger<RequestContextMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, IUserContextRepository userContext)
    {
        try
        {
            // Log informações do contexto da requisição
            if (userContext.IsAuthenticated())
            {
                var userId = userContext.GetCurrentUserId();
                var userName = userContext.GetCurrentUserName();
                
                _logger.LogInformation("Requisição autenticada - UserId: {UserId}, UserName: {UserName}", 
                    userId, userName);
                
                // Adicionar headers de contexto se necessário
                context.Response.Headers.Add("X-User-Context", userId ?? "anonymous");
            }
            else
            {
                _logger.LogInformation("Requisição não autenticada - Path: {Path}", context.Request.Path);
            }

            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Erro no middleware de contexto da requisição");
            throw;
        }
    }
}