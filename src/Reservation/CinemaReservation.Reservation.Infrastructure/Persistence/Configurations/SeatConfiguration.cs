using CinemaReservation.Reservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CinemaReservation.Reservation.Infrastructure.Persistence.Configurations;

public class SeatConfiguration : IEntityTypeConfiguration<Seat>
{
    public void Configure(EntityTypeBuilder<Seat> builder)
    {
        builder.ToTable("Seats");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.SeatNo).IsRequired().HasMaxLength(20);
        builder.Property(x => x.ShowtimeId).IsRequired();
        builder.Property(x => x.IsReserved).IsRequired();
        builder.HasIndex(x => new { x.HallId, x.ShowtimeId, x.SeatNo }).IsUnique();
    }
}
