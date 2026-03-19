using Asperitas.Api.Application.Common.Repositories;
using Asperitas.Api.Application.Models;
using Asperitas.Api.Domain.Entities;
using Asperitas.Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Asperitas.Api.Infrastructure.Repositories;

public class PostRepository(AppDbContext dbContext) : IPostRepository
{
    private readonly AppDbContext _dbContext = dbContext;

    public async Task<Guid> CreateAsync(Post post, CancellationToken cancellationToken)
    {
        await _dbContext.Posts.AddAsync(post, cancellationToken);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return post.Id;
    }

    public async Task DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
        var deleted = await _dbContext.Posts
            .Where(x => x.Id == id)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleted == 0)
            throw new KeyNotFoundException("Post not found");
    }

    public async Task IncrementViewsAsync(Guid id, CancellationToken cancellationToken)
    {
        var updated = await _dbContext.Posts
            .Where(p => p.Id == id)
            .ExecuteUpdateAsync(
                s => s.SetProperty(p => p.Views, p => p.Views + 1),
                cancellationToken
            );

        if (updated == 0)
            throw new KeyNotFoundException("Post not found");
    }

    public Task<PostDetailsWithComments> GetPostDetailsAsync(
        Guid id,
        Guid? currentUserId,
        CancellationToken cancellationToken
    )
    {
        return GetPostDetailsByIdAsync(id, currentUserId, cancellationToken);
    }

    public async Task<List<PostDetails>> GetPostsAsync(
        string? category,
        string? username,
        Guid? currentUserId,
        CancellationToken cancellationToken
    )
    {
        return await _dbContext.Posts
            .Where(p =>
#pragma warning disable CA1862
                (string.IsNullOrWhiteSpace(category) || p.Category.ToLower() == category.ToLower()) &&
#pragma warning restore CA1862
                (string.IsNullOrWhiteSpace(username) || p.Author.Username == username))
            .Select(p => new PostDetails
            {
                Id = p.Id,
                Category = p.Category,
                Title = p.Title,
                Type = p.Type,
                Created = p.Created,
                Text = p.Text,
                Url = p.Url,
                Views = p.Views,
                CommentsCount = p.Comments.Count,
                Score = p.Votes.Sum(v => v.UserVote),

                UpvotePercentage = p.Votes.Count() == 0
                    ? 0
                    : (int)Math.Round(p.Votes.Count(v => v.UserVote == 1) * 100.0 / p.Votes.Count()),

                CurrentUserVote = currentUserId == null
                    ? 0
                    : p.Votes
                        .Where(v => v.UserId == currentUserId)
                        .Select(v => v.UserVote)
                        .FirstOrDefault(),

                Author = new UserDetails
                {
                    Id = p.Author.Id,
                    Username = p.Author.Username
                }
            })
            .ToListAsync(cancellationToken);
    }

    public async Task<PostDetailsWithComments> AddCommentAsync(
        Guid postId,
        Guid authorId,
        string body,
        CancellationToken cancellationToken
    )
    {
        var post = await _dbContext.Posts
            .Include(p => p.Votes)
            .Include(p => p.Comments)
            .Include(p => p.Author)
            .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken)
            ?? throw new KeyNotFoundException("Post not found");

        var comment = new Comment
        {
            Id = Guid.NewGuid(),
            AuthorId = authorId,
            PostId = post.Id,
            Body = body,
        };

        _dbContext.Comments.Add(comment);
        post.Comments.Add(comment);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _dbContext.Entry(post)
            .Collection(p => p.Comments)
            .Query()
            .Include(p => p.Author)
            .LoadAsync(cancellationToken: cancellationToken);

        return MapPostDetails(post, authorId);
    }

    public async Task<PostDetailsWithComments> DeleteCommentAsync(
        Guid postId,
        Guid commentId,
        Guid currentUserId,
        CancellationToken cancellationToken
    )
    {
        var deleted = await _dbContext.Comments
            .Where(c => c.Id == commentId && c.PostId == postId)
            .ExecuteDeleteAsync(cancellationToken);

        if (deleted == 0)
            throw new KeyNotFoundException("Comment not found");

        return await GetPostDetailsByIdAsync(postId, currentUserId, cancellationToken);
    }

    public async Task<PostDetailsWithComments> UpvoteAsync(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        await EnsurePostExistsAsync(postId, cancellationToken);

        var existingVote = await _dbContext.Votes
            .FirstOrDefaultAsync(v => v.PostId == postId && v.UserId == userId, cancellationToken);

        if (existingVote is not null && existingVote.UserVote == 1)
            throw new InvalidOperationException("User already upvoted");

        if (existingVote is not null && existingVote.UserVote == -1)
            existingVote.UserVote = 1;
        else
        {
            _dbContext.Votes.Add(new Vote
            {
                PostId = postId,
                UserId = userId,
                UserVote = 1
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetPostDetailsByIdAsync(postId, userId, cancellationToken);
    }

    public async Task<PostDetailsWithComments> DownvoteAsync(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        await EnsurePostExistsAsync(postId, cancellationToken);

        var existingVote = await _dbContext.Votes
            .FirstOrDefaultAsync(v => v.PostId == postId && v.UserId == userId, cancellationToken);

        if (existingVote is not null && existingVote.UserVote == -1)
            throw new InvalidOperationException("User already downvoted");

        if (existingVote is not null && existingVote.UserVote == 1)
            existingVote.UserVote = -1;
        else
        {
            _dbContext.Votes.Add(new Vote
            {
                PostId = postId,
                UserId = userId,
                UserVote = -1
            });
        }

        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetPostDetailsByIdAsync(postId, userId, cancellationToken);
    }

    public async Task<PostDetailsWithComments> UnvoteAsync(Guid postId, Guid userId, CancellationToken cancellationToken)
    {
        await EnsurePostExistsAsync(postId, cancellationToken);

        var existingVote = await _dbContext.Votes
            .FirstOrDefaultAsync(v => v.PostId == postId && v.UserId == userId, cancellationToken)
            ?? throw new KeyNotFoundException("Vote not found");

        _dbContext.Votes.Remove(existingVote);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return await GetPostDetailsByIdAsync(postId, userId, cancellationToken);
    }

    private async Task EnsurePostExistsAsync(Guid postId, CancellationToken cancellationToken)
    {
        var exists = await _dbContext.Posts
            .AnyAsync(p => p.Id == postId, cancellationToken);

        if (!exists)
            throw new KeyNotFoundException("Post not found");
    }

    private async Task<PostDetailsWithComments> GetPostDetailsByIdAsync(
        Guid postId,
        Guid? currentUserId,
        CancellationToken cancellationToken
    )
    {
        var post = await _dbContext.Posts
            .Include(p => p.Votes)
            .Include(p => p.Author)
            .Include(p => p.Comments)
                .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(p => p.Id == postId, cancellationToken)
            ?? throw new KeyNotFoundException("Post not found");

        return MapPostDetails(post, currentUserId);
    }

    private static PostDetailsWithComments MapPostDetails(Post post, Guid? currentUserId)
    {
        return new PostDetailsWithComments
        {
            Id = post.Id,
            Category = post.Category,
            Title = post.Title,
            Type = post.Type,
            Created = post.Created,
            Text = post.Text,
            Url = post.Url,
            Views = post.Views,
            CommentsCount = post.Comments.Count,
            Score = post.Votes.Sum(v => v.UserVote),

            UpvotePercentage = post.Votes.Count == 0
                ? 0
                : (int)Math.Round(post.Votes.Count(v => v.UserVote == 1) * 100.0 / post.Votes.Count),

            CurrentUserVote = currentUserId == null
                ? 0
                : post.Votes
                    .Where(v => v.UserId == currentUserId)
                    .Select(v => v.UserVote)
                    .FirstOrDefault(),

            Author = new UserDetails
            {
                Id = post.Author.Id,
                Username = post.Author.Username
            },

            Comments = post.Comments
                .OrderBy(c => c.Created)
                .Select(c => new CommentDetails
                {
                    Id = c.Id,
                    Body = c.Body,
                    Created = c.Created,
                    Author = new UserDetails
                    {
                        Id = c.Author.Id,
                        Username = c.Author.Username
                    }
                }).ToList()
        };
    }
}
