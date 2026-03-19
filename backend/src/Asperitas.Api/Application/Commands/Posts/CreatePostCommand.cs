using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Domain.Entities;
using MediatR;

namespace Asperitas.Api.Application.Commands.Posts;

public record CreatePostCommand(
    string Title,
    string Text,
    string Url,
    string Type,
    string Category
) : IRequest<Guid>;

public class CreatePostHandler(IPostRepository postRepository, ICurrentUserService currentUserService) : IRequestHandler<CreatePostCommand, Guid>
{
    public async Task<Guid> Handle(CreatePostCommand request, CancellationToken cancellationToken)
    {
        var authorId = currentUserService.UserId
            ?? throw new UnauthorizedAccessException("Not authorized");

        var post = new Post
        {
            Id = Guid.NewGuid(),
            AuthorId = authorId,
            Title = request.Title,
            Text = request.Text,
            Type = request.Type,
            Created = DateTime.UtcNow,
            Category = request.Category,
            Url = request.Url,
        };

        return await postRepository.CreateAsync(post, cancellationToken);
    }
}
