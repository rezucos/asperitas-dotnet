namespace Asperitas.Api.Presentation.Contracts.Responses;

public class CommentDto
{
    public required Guid Id { get; set; }
    public required string Body { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public UserDto Author { get; set; } = default!;
}
