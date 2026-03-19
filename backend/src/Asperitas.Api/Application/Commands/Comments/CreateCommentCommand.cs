using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Models;
using MediatR;

namespace Asperitas.Api.Application.Commands.Comments;

public record CreateCommentCommand(Guid PostId, string Comment) : IRequest<PostDetailsWithComments>;

class CreateCommentHandler(IPostRepository postRepository, ICurrentUserService currentUserService) : IRequestHandler<CreateCommentCommand, PostDetailsWithComments>
{
    public async Task<PostDetailsWithComments> Handle(CreateCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Not authorized");

        return await postRepository.AddCommentAsync(
            request.PostId,
            userId,
            request.Comment,
            cancellationToken
        );
    }
}
