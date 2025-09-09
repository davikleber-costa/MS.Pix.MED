using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace MS.Pix.MED.Api.Controllers;

[ApiController]
[AllowAnonymous] // Cognito precisa acessar sem autenticação
[Route("v1/cognito")]
[Produces("application/json")]
public class CognitoWebhookController : ControllerBase
{
	private readonly ILogger<CognitoWebhookController> _logger;

	public CognitoWebhookController(ILogger<CognitoWebhookController> logger)
	{
		_logger = logger;
	}

	/// <summary>
	/// Webhook para eventos de pré-autenticação do Cognito
	/// </summary>
	[HttpPost("pre-authentication")]
	[ProducesResponseType(typeof(CognitoResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> PreAuthentication([FromBody] CognitoPreAuthRequest request)
	{
		try
		{
			_logger.LogInformation("Cognito Pre-Authentication trigger received for user: {Username}",
				request?.UserName ?? "Unknown");

			// Aqui você pode implementar validações customizadas
			// Por exemplo: verificar se o usuário está ativo, validar domínio do email, etc.

			var response = new CognitoResponse
			{
				UserName = request?.UserName,
				Response = new Dictionary<string, object>
				{
					{ "finalUserStatus", "CONFIRMED" },
					{ "messageAction", "SUPPRESS" }
				}
			};

			return Ok(response);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error processing Cognito pre-authentication");
			return BadRequest(new { error = "Invalid request" });
		}
	}

	/// <summary>
	/// Webhook para eventos de pós-autenticação do Cognito
	/// </summary>
	[HttpPost("post-authentication")]
	[ProducesResponseType(typeof(CognitoResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> PostAuthentication([FromBody] CognitoPostAuthRequest request)
	{
		try
		{
			_logger.LogInformation("Cognito Post-Authentication trigger received for user: {Username}",
				request?.UserName ?? "Unknown");

			// Aqui você pode implementar lógicas pós-login
			// Por exemplo: registrar último login, atualizar estatísticas, etc.

			var response = new CognitoResponse
			{
				UserName = request?.UserName,
				Response = new Dictionary<string, object>()
			};

			return Ok(response);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error processing Cognito post-authentication");
			return BadRequest(new { error = "Invalid request" });
		}
	}

	/// <summary>
	/// Webhook para eventos de pré-signup do Cognito
	/// </summary>
	[HttpPost("pre-signup")]
	[ProducesResponseType(typeof(CognitoResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> PreSignUp([FromBody] CognitoPreSignUpRequest request)
	{
		try
		{
			_logger.LogInformation("Cognito Pre-SignUp trigger received for user: {Username}",
				request?.UserName ?? "Unknown");

			// Aqui você pode implementar validações de registro
			// Por exemplo: validar formato do email, verificar domínios permitidos, etc.

			var response = new CognitoResponse
			{
				UserName = request?.UserName,
				Response = new Dictionary<string, object>
				{
					{ "autoConfirmUser", true },
					{ "autoVerifyEmail", true }
				}
			};

			return Ok(response);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error processing Cognito pre-signup");
			return BadRequest(new { error = "Invalid request" });
		}
	}

	/// <summary>
	/// Webhook para eventos de pós-confirmação do Cognito
	/// </summary>
	[HttpPost("post-confirmation")]
	[ProducesResponseType(typeof(CognitoResponse), StatusCodes.Status200OK)]
	[ProducesResponseType(StatusCodes.Status400BadRequest)]
	public async Task<IActionResult> PostConfirmation([FromBody] CognitoPostConfirmationRequest request)
	{
		try
		{
			_logger.LogInformation("Cognito Post-Confirmation trigger received for user: {Username}",
				request?.UserName ?? "Unknown");

			// Aqui você pode implementar lógicas pós-confirmação
			// Por exemplo: criar perfil do usuário, enviar email de boas-vindas, etc.

			var response = new CognitoResponse
			{
				UserName = request?.UserName,
				Response = new Dictionary<string, object>()
			};

			return Ok(response);
		}
		catch (Exception ex)
		{
			_logger.LogError(ex, "Error processing Cognito post-confirmation");
			return BadRequest(new { error = "Invalid request" });
		}
	}

	/// <summary>
	/// Health check para verificar se o serviço está funcionando
	/// </summary>
	[HttpGet("health")]
	[ProducesResponseType(StatusCodes.Status200OK)]
	public IActionResult Health()
	{
		return Ok(new { status = "healthy", timestamp = DateTime.UtcNow });
	}
}

// Models para os requests do Cognito
public class CognitoPreAuthRequest
{
	public string? UserName { get; set; }
	public string? TriggerSource { get; set; }
	public Dictionary<string, object>? Request { get; set; }
	public Dictionary<string, string>? UserAttributes { get; set; }
}

public class CognitoPostAuthRequest
{
	public string? UserName { get; set; }
	public string? TriggerSource { get; set; }
	public Dictionary<string, object>? Request { get; set; }
	public Dictionary<string, string>? UserAttributes { get; set; }
}

public class CognitoPreSignUpRequest
{
	public string? UserName { get; set; }
	public string? TriggerSource { get; set; }
	public Dictionary<string, object>? Request { get; set; }
	public Dictionary<string, string>? UserAttributes { get; set; }
}

public class CognitoPostConfirmationRequest
{
	public string? UserName { get; set; }
	public string? TriggerSource { get; set; }
	public Dictionary<string, object>? Request { get; set; }
	public Dictionary<string, string>? UserAttributes { get; set; }
}

public class CognitoResponse
{
	public string? UserName { get; set; }
	public Dictionary<string, object> Response { get; set; } = new();
}