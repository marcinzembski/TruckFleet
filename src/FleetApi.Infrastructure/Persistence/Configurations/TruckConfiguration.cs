namespace FleetApi.Infrastructure.Persistence.Configurations;

using FleetApi.Domain.Trucks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

public class TruckConfiguration : IEntityTypeConfiguration<Truck>
{
    public void Configure(EntityTypeBuilder<Truck> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion(v => v.Value, v => TruckId.From(v));

        builder.Property(t => t.Code)
            .HasConversion(v => v.Value, v => TruckCode.Create(v))
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(t => t.Code).IsUnique();

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.Status)
            .IsRequired()
            .HasConversion<string>();

        builder.Property(t => t.Description)
            .HasMaxLength(500);

        builder.Property(t => t.CreatedAt).IsRequired();
        builder.Property(t => t.UpdatedAt).IsRequired();
    }
}
