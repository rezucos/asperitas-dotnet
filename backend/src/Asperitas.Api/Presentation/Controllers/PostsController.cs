using Asperitas.Api.Application.Commands.Posts;
using Asperitas.Api.Application.Queries.Posts;
using Asperitas.Api.Presentation.Contracts.Requests;
using Asperitas.Api.Presentation.Contracts.Responses;
using Asperitas.Api.Presentation.Mapping;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Asperitas.Api.Presentation.Controllers;

[ApiController]
[Route("/api/posts")]
public class PostsController(IMediator mediator) : ControllerBase
{
    private readonly IMediator _mediator = mediator;

    [HttpGet]
    public async Task<List<PostDto>> GetAll(CancellationToken cancellationToken, [FromQuery] string? category = null)
    {
        var query = new GetPostsQuery(category);
        var posts = await _mediator.Send(query, cancellationToken: cancellationToken);
        return posts.ToPostDtoList();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> CreatePost([FromBody] CreatePostRequest postRequest, CancellationToken cancellationToken)
    {
        var postId = await _mediator.Send(postRequest.ToCreatePostCommand(), cancellationToken: cancellationToken);
        return StatusCode(201, new { id = postId });
    }

    [HttpGet("{id:guid}")]
    public async Task<PostDetailsDto> ViewPostById(Guid id, CancellationToken cancellationToken)
    {
        var query = new ViewPostCommand(id);
        var post = await _mediator.Send(query, cancellationToken: cancellationToken);
        return post.ToPostDetailsDto();
    }

    [Authorize]
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeletePostById(Guid id, CancellationToken cancellationToken)
    {
        var command = new DeletePostCommand(id);
        await _mediator.Send(command, cancellationToken: cancellationToken);
        return StatusCode(200, new { message = "success" });
    }

    [Authorize]
    [HttpPost("{id:guid}/upvote")]
    public async Task<PostDetailsDto> UpvotePost(Guid id, CancellationToken cancellationToken)
    {
        var command = new UpvotePostCommand(id);
        var post = await _mediator.Send(command, cancellationToken: cancellationToken);
        return post.ToPostDetailsDto();
    }

    [Authorize]
    [HttpPost("{id:guid}/downvote")]
    public async Task<PostDetailsDto> DownvotePost(Guid id, CancellationToken cancellationToken)
    {
        var command = new DownvotePostCommand(id);
        var post = await _mediator.Send(command, cancellationToken: cancellationToken);
        return post.ToPostDetailsDto();
    }

    [Authorize]
    [HttpPost("{id:guid}/unvote")]
    public async Task<PostDetailsDto> UnvotePost(Guid id, CancellationToken cancellationToken)
    {
        var command = new UnvotePostCommand(id);
        var post = await _mediator.Send(command, cancellationToken: cancellationToken);
        return post.ToPostDetailsDto();
    }
}
