namespace Asperitas.Api.Application.Models;

public class CommentDetails
{
    public required Guid Id { get; set; }
    public required string Body { get; set; }
    public DateTime Created { get; set; }

    public UserDetails Author { get; set; } = default!;
}
