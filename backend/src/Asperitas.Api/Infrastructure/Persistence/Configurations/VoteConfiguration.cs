using Asperitas.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asperitas.Api.Infrastructure.Persistence.Configurations;

public class VoteConfiguration : IEntityTypeConfiguration<Vote>
{
    public void Configure(EntityTypeBuilder<Vote> builder)
    {
        builder.ToTable("votes",
                t => t.HasCheckConstraint(
                    "ck_votes_user_vote",
                    "user_vote = 1 OR user_vote = -1"
                ))
                .HasKey(e => new { e.UserId, e.PostId });

        builder.HasOne(e => e.User)
            .WithMany(e => e.Votes)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Post)
            .WithMany(e => e.Votes)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.UserId)
            .HasColumnName("user_id")
            .IsRequired();

        builder.Property(e => e.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(e => e.UserVote)
            .HasColumnName("user_vote")
            .IsRequired();
    }
}
