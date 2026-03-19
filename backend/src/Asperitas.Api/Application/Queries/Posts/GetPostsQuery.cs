using Asperitas.Api.Application.Common.Interfaces;
using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Models;
using MediatR;

namespace Asperitas.Api.Application.Queries.Posts;

public record GetPostsQuery(
    string? Category = null,
    string? Username = null
) : IRequest<List<PostDetails>>;

public class GetPostsHandler(IPostRepository postRepository, ICurrentUserService currentUserService) : IRequestHandler<GetPostsQuery, List<PostDetails>>
{
    public async Task<List<PostDetails>> Handle(GetPostsQuery request, CancellationToken cancellationToken)
    {
        var userId = currentUserService.UserId;
        return await postRepository.GetPostsAsync(
            request.Category,
            request.Username,
            userId,
            cancellationToken
        );
    }
}
