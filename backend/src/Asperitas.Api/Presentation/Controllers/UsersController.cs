using Asperitas.Api.Application.Queries.Posts;
using Asperitas.Api.Presentation.Mapping;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Asperitas.Api.Presentation.Controllers;

[ApiController]
[Route("/api/users")]
public class UsersController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    [Route("{username}")]
    public async Task<IActionResult> GetUser(string username, CancellationToken cancellationToken)
    {
        var query = new GetPostsQuery(Username: username);
        var userPosts = await _mediator.Send(query, cancellationToken);
        return StatusCode(200, userPosts.ToPostDtoList());
    }
}
