using Asperitas.Api.Presentation.Contracts.Requests;
using Asperitas.Api.Presentation.Mapping;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asperitas.Api.Presentation.Controllers;

[ApiController]
[Route("/api/auth")]
public class AuthController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CreateUserRequest request, CancellationToken cancellationToken)
    {
        var token = await _mediator.Send(request.ToCreateUserCommand(), cancellationToken);
        return StatusCode(201, new { token });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request, CancellationToken cancellationToken)
    {
        var token = await _mediator.Send(request.ToLoginCommand(), cancellationToken);
        return StatusCode(200, new { token });
    }
}
