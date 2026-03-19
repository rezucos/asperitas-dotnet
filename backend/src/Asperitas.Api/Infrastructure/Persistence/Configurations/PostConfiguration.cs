using Asperitas.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asperitas.Api.Infrastructure.Persistence.Configurations;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> builder)
    {
        builder.ToTable("posts")
                .HasKey(e => e.Id);

        builder.HasIndex(e => e.Category);

        builder.HasOne(e => e.Author)
            .WithMany(e => e.Posts)
            .HasForeignKey(e => e.AuthorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.AuthorId)
            .HasColumnName("author_id")
            .IsRequired();

        builder.Property(e => e.Title)
            .HasColumnName("title")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.Text)
            .HasColumnName("text")
            .HasMaxLength(2000);

        builder.Property(e => e.Url)
            .HasColumnName("url")
            .HasMaxLength(250);

        builder.Property(e => e.Created)
            .HasColumnName("created");

        builder.Property(e => e.Type)
            .HasColumnName("type")
            .HasMaxLength(10)
            .IsRequired();

        builder.Property(e => e.Category)
            .HasColumnName("category")
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(e => e.Views)
            .HasColumnName("views");
    }
}
