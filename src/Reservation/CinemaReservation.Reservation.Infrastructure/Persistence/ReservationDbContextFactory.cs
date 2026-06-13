using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace CinemaReservation.Reservation.Infrastructure.Persistence;

public class ReservationDbContextFactory : IDesignTimeDbContextFactory<ReservationDbContext>
{
    public ReservationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ReservationDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5433;Database=reservationdb;Username=postgres;Password=postgres");

        return new ReservationDbContext(optionsBuilder.Options);
    }
}
