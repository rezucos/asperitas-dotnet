using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Models;
using MediatR;

namespace Asperitas.Api.Application.Commands.Posts;

public record UnvotePostCommand(Guid Id) : IRequest<PostDetailsWithComments>;

class UnvotePostHandler(
    IPostRepository postRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<UnvotePostCommand, PostDetailsWithComments>
{
    public async Task<PostDetailsWithComments> Handle(UnvotePostCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Not authorized");

        return await postRepository.UnvoteAsync(request.Id, userId, cancellationToken);
    }
}
