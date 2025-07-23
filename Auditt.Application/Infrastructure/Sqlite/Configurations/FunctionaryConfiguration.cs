using Auditt.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auditt.Application.Infrastructure.Sqlite.Configurations;

public class FunctionaryConfiguration : IEntityTypeConfiguration<Functionary>
{
    public void Configure(EntityTypeBuilder<Functionary> builder)
    {
        builder.ToTable("Funcionarios");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.FirstName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(f => f.LastName)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(f => f.Identification)
            .IsRequired()
            .HasMaxLength(20);

        builder.HasIndex(f => f.Identification)
            .IsUnique();
    }
}
