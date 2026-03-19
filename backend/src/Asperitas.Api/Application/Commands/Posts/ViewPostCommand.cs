using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Models;
using MediatR;

namespace Asperitas.Api.Application.Commands.Posts;

public record ViewPostCommand(Guid Id) : IRequest<PostDetailsWithComments>;

public class ViewPostHandler(
    IPostRepository postRepository,
    ICurrentUserService currentUserService
) : IRequestHandler<ViewPostCommand, PostDetailsWithComments>
{
    public async Task<PostDetailsWithComments> Handle(ViewPostCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;

        if (userId is not null)
        {
            await postRepository.IncrementViewsAsync(request.Id, cancellationToken);
        }

        return await postRepository.GetPostDetailsAsync(request.Id, userId, cancellationToken);
    }
}
