namespace Asperitas.Api.Domain.Entities;

public class Vote
{
    public required Guid UserId { get; set; }
    public required Guid PostId { get; set; }
    public required int UserVote { get; set; }

    public User User { get; set; } = default!;
    public Post Post { get; set; } = default!;
}
