using Asperitas.Api.Application.Models;
using Asperitas.Api.Domain.Entities;

namespace Asperitas.Api.Application.Common.Repositories;

public interface IPostRepository
{
    Task<Guid> CreateAsync(Post post, CancellationToken cancellationToken);
    Task DeleteAsync(Guid id, CancellationToken cancellationToken);
    Task IncrementViewsAsync(Guid id, CancellationToken cancellationToken);
    Task<PostDetailsWithComments> GetPostDetailsAsync(Guid id, Guid? currentUserId, CancellationToken cancellationToken);
    Task<List<PostDetails>> GetPostsAsync(string? category, string? username, Guid? currentUserId, CancellationToken cancellationToken);
    Task<PostDetailsWithComments> AddCommentAsync(Guid postId, Guid authorId, string body, CancellationToken cancellationToken);
    Task<PostDetailsWithComments> DeleteCommentAsync(Guid postId, Guid commentId, Guid currentUserId, CancellationToken cancellationToken);
    Task<PostDetailsWithComments> UpvoteAsync(Guid postId, Guid userId, CancellationToken cancellationToken);
    Task<PostDetailsWithComments> DownvoteAsync(Guid postId, Guid userId, CancellationToken cancellationToken);
    Task<PostDetailsWithComments> UnvoteAsync(Guid postId, Guid userId, CancellationToken cancellationToken);
}
