using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Models;
using MediatR;

namespace Asperitas.Api.Application.Commands.Posts;

public record DownvotePostCommand(Guid Id) : IRequest<PostDetailsWithComments>;

class DownvotePostHandler(
    IPostRepository postRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<DownvotePostCommand, PostDetailsWithComments>
{
    public async Task<PostDetailsWithComments> Handle(DownvotePostCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Not authorized");

        return await postRepository.DownvoteAsync(request.Id, userId, cancellationToken);
    }
}
