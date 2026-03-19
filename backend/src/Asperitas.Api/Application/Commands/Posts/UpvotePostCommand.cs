using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Models;
using MediatR;

namespace Asperitas.Api.Application.Commands.Posts;

public record UpvotePostCommand(Guid Id) : IRequest<PostDetailsWithComments>;

class UpvotePostHandler(
    IPostRepository postRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<UpvotePostCommand, PostDetailsWithComments>
{
    public async Task<PostDetailsWithComments> Handle(UpvotePostCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Not authorized");

        return await postRepository.UpvoteAsync(request.Id, userId, cancellationToken);
    }
}
