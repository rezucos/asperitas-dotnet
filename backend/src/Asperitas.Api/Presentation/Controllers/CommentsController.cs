using Asperitas.Api.Application.Commands.Comments;
using Asperitas.Api.Presentation.Contracts.Requests;
using Asperitas.Api.Presentation.Mapping;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asperitas.Api.Presentation.Controllers;

[ApiController]
[Route("api/posts/{postId:guid}")]
public class CommentsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpPost("comment")]
    public async Task<IActionResult> CreateComment(Guid postId, [FromBody] CreateCommentRequest request, CancellationToken cancellationToken)
    {
        var command = new CreateCommentCommand(postId, request.Comment);
        var post = await _mediator.Send(command, cancellationToken: cancellationToken);
        return StatusCode(201, post.ToPostDetailsDto());
    }

    [Authorize]
    [HttpDelete("comment/{commentId:guid}")]
    public async Task<IActionResult> DeleteComment(Guid postId, Guid commentId, CancellationToken cancellationToken)
    {
        var command = new DeleteCommentCommand(postId, commentId);
        var post = await _mediator.Send(command, cancellationToken: cancellationToken);
        return StatusCode(200, post.ToPostDetailsDto());
    }
}
