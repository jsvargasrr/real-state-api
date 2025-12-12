using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using RealEstate.Domain.Entities;

namespace RealEstate.Infrastructure.Configurations;

public class ReservationConfiguration : IEntityTypeConfiguration<Reservation>
{
    public void Configure(EntityTypeBuilder<Reservation> builder)
    {
        builder.ToTable("Reservations");

        builder.HasKey(r => r.IdReservation);

        builder.Property(r => r.GuestName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.GuestEmail)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(r => r.TotalPrice)
            .HasPrecision(18, 2);

        builder.Property(r => r.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(r => r.Property)
            .WithMany(p => p.Reservations)
            .HasForeignKey(r => r.IdProperty)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
