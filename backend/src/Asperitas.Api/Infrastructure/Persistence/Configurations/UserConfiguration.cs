using Asperitas.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Asperitas.Api.Infrastructure.Persistence.Configurations;

public class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users")
            .HasKey(e => e.Id);

        builder.HasIndex(e => e.Username)
            .IsUnique();

        builder.Property(e => e.Id)
            .HasColumnName("id")
            .IsRequired();

        builder.Property(e => e.Username)
            .HasColumnName("username")
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(e => e.Password)
            .HasColumnName("password")
            .HasMaxLength(300)
            .IsRequired();

        builder.Property(e => e.IsAdmin)
            .HasColumnName("is_admin")
            .HasColumnType("boolean");
    }
}
