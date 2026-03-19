namespace Asperitas.Api.Domain.Entities;

public class Comment
{
    public required Guid Id { get; set; }
    public required Guid AuthorId { get; set; }
    public required Guid PostId { get; set; }
    public required string Body { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public User Author { get; set; } = default!;
    public Post Post { get; set; } = default!;
}
