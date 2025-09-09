using Microsoft.AspNetCore.Mvc;

namespace MS.Pix.MED.Api.Controllers;

[ApiController]
[Route("v{version:apiVersion}/med/[controller]")]
[Produces("application/json")]
public abstract class MedControllerBase : ControllerBase
{
}