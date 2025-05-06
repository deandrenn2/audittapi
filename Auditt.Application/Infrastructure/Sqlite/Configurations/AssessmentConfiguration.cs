using Auditt.Application.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Auditt.Application.Infrastructure.Sqlite.Configurations;

internal class AssessmentConfiguration : IEntityTypeConfiguration<Assessment>
{
    public void Configure(EntityTypeBuilder<Assessment> builder)
    {
        builder.ToTable("Assessments");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(x => x.InstitutionId).IsRequired();
        builder.Property(x => x.DataCutId).IsRequired();
        builder.Property(x => x.FunctionaryId).IsRequired();
        builder.Property(x => x.PatientId).IsRequired();
        builder.Property(x => x.GuideId).IsRequired();
        builder.Property(x => x.YearOld).IsRequired();
        builder.Property(x => x.Date).IsRequired();
        builder.Property(x => x.Eps).IsRequired();
        builder.Property(x => x.IdUserCreated).IsRequired();
        builder.Property(x => x.IdUserUpdate).IsRequired();
        builder.Property(x => x.UpdateDate).IsRequired();
        builder.Property(x => x.CreateDate).IsRequired();
        builder.HasIndex(x => new { x.InstitutionId, x.DataCutId, x.FunctionaryId, x.PatientId, x.GuideId }).IsUnique();
    }
}