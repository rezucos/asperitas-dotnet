namespace Asperitas.Api.Application.Models;

public class PostDetails
{
    public required Guid Id { get; set; }
    public required string Title { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public required string Type { get; set; } = "link";
    public required string Category { get; set; }
    public int Views { get; set; }
    public int Score { get; set; }
    public int CommentsCount { get; set; }
    public int CurrentUserVote { get; set; }
    public int UpvotePercentage { get; set; }

    public UserDetails Author { get; set; } = default!;
}

public class PostDetailsWithComments : PostDetails
{
    public List<CommentDetails> Comments { get; set; } = [];
}
