namespace Asperitas.Api.Domain.Entities;

public class User
{
    public required Guid Id { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
    public bool IsAdmin { get; set; } = default;

    public List<Comment> Comments { get; set; } = [];
    public List<Post> Posts { get; set; } = [];
    public List<Vote> Votes { get; set; } = [];
}
