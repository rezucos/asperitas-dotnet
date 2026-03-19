using Asperitas.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asperitas.Api.Infrastructure.Persistence.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comments")
                .HasKey(e => e.Id);

        builder.HasIndex(e => e.PostId);

        builder.HasOne(e => e.Author)
            .WithMany(e => e.Comments)
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(e => e.Post)
            .WithMany(e => e.Comments)
            .HasForeignKey(e => e.PostId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.AuthorId)
            .HasColumnName("author_id")
            .IsRequired();

        builder.Property(e => e.PostId)
            .HasColumnName("post_id")
            .IsRequired();

        builder.Property(e => e.Body)
            .HasColumnName("body")
            .HasMaxLength(2000)
            .IsRequired();

        builder.Property(e => e.Created)
            .HasColumnName("created");
    }
}
