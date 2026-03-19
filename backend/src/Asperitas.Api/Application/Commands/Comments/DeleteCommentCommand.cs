using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Models;
using MediatR;

namespace Asperitas.Api.Application.Commands.Comments;

public record DeleteCommentCommand(Guid PostId, Guid CommentId) : IRequest<PostDetailsWithComments>;

class DeleteCommentHandler(IPostRepository postRepository, ICurrentUserService currentUserService) : IRequestHandler<DeleteCommentCommand, PostDetailsWithComments>
{
    public async Task<PostDetailsWithComments> Handle(DeleteCommentCommand request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Not authorized");

        return await postRepository.DeleteCommentAsync(
            request.PostId,
            request.CommentId,
            userId,
            cancellationToken
        );
    }
}
