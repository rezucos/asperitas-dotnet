namespace Asperitas.Api.Domain.Entities;

public class Post
{
    public required Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required string Title { get; set; }
    public string Text { get; set; } = string.Empty;
    public string Url { get; set; } = string.Empty;
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public required string Type { get; set; } = "link";
    public required string Category { get; set; }
    public int Views { get; set; }

    public User Author { get; set; } = default!;
    public List<Comment> Comments { get; set; } = [];
    public List<Vote> Votes { get; set; } = [];
}
