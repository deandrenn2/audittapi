using Auditt.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auditt.Application.Infrastructure.Sqlite.Configurations;

internal class DataCutConfiguration : IEntityTypeConfiguration<DataCut>
{
    public void Configure(EntityTypeBuilder<DataCut> builder)
    {
        builder.ToTable("DataCuts");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.Name).IsRequired().HasMaxLength(100);
        builder.Property(x => x.Description).HasMaxLength(500);
    }
}

