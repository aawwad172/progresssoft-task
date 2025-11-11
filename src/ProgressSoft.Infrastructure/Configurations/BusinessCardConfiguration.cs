using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProgressSoft.Domain.Constants.Infrastructure;
using ProgressSoft.Domain.Entities;

namespace ProgressSoft.Infrastructure.Configurations;

public class BusinessCardConfiguration : IEntityTypeConfiguration<BusinessCard>
{
    public void Configure(EntityTypeBuilder<BusinessCard> builder)
    {
        builder.HasKey(bc => bc.Id);

        builder.Property(bc => bc.Name)
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(bc => bc.Gender)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(bc => bc.DateOfBirth)
            .IsRequired();

        builder.Property(bc => bc.Email)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(bc => bc.Phone)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(bc => bc.Address)
            .IsRequired()
            .HasMaxLength(500);

        builder.Property(bc => bc.PhotoBase64)
            .IsRequired(false);

        builder.Property(bc => bc.CreatedAt)
            .HasDefaultValueSql(DbConstantsFunctions.UtcNow)
            .IsRequired();

        builder.Property(bc => bc.IsDeleted)
            .HasDefaultValue(false);
    }
}
