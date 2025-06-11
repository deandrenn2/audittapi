using Auditt.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auditt.Application.Infrastructure.Sqlite.Configurations;

public class InstitutionConfiguration : IEntityTypeConfiguration<Institution>
{
    public void Configure(EntityTypeBuilder<Institution> builder)
    {
        builder.ToTable("Instituciones");

        builder.HasKey(i => i.Id);

        builder.Property(i => i.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(i => i.Nit)
            .IsRequired()
            .HasMaxLength(200);

        builder.HasIndex(i => i.Nit)
            .IsUnique();
    }
}
