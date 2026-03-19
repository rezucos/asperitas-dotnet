using Asperitas.Api.Application.Models;
using Asperitas.Api.Presentation.Contracts.Responses;

namespace Asperitas.Api.Presentation.Mapping;

public static class ModelToApiContractMapper
{
    public static List<PostDto> ToPostDtoList(this IEnumerable<PostDetails> posts)
    {
        return posts.Select(ToPostDto).ToList();
    }

    public static PostDto ToPostDto(this PostDetails post)
    {
        return new PostDto
        {
            Id = post.Id,
            Category = post.Category,
            Title = post.Title,
            Type = post.Type,
            Created = post.Created,
            Text = post.Text,
            Url = post.Url,
            Views = post.Views,
            CommentsCount = post.CommentsCount,
            Score = post.Score,
            UpvotePercentage = post.UpvotePercentage,
            CurrentUserVote = post.CurrentUserVote,
            Author = new UserDto
            {
                Id = post.Author.Id,
                Username = post.Author.Username
            }
        };
    }

    public static PostDetailsDto ToPostDetailsDto(this PostDetailsWithComments post)
    {
        return new PostDetailsDto
        {
            Id = post.Id,
            Category = post.Category,
            Title = post.Title,
            Type = post.Type,
            Created = post.Created,
            Text = post.Text,
            Url = post.Url,
            Views = post.Views,
            CommentsCount = post.CommentsCount,
            Score = post.Score,
            UpvotePercentage = post.UpvotePercentage,
            CurrentUserVote = post.CurrentUserVote,
            Author = new UserDto
            {
                Id = post.Author.Id,
                Username = post.Author.Username
            },
            Comments = post.Comments
                .OrderBy(c => c.Created)
                .Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    Created = c.Created,
                    Author = new UserDto
                    {
                        Id = c.Author.Id,
                        Username = c.Author.Username
                    }
                }).ToList()
        };
    }
}
