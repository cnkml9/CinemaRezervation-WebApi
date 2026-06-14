using CinemaReservation.Reservation.Application.Abstractions;
using CinemaReservation.Reservation.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Reservation.Infrastructure.Persistence;

public class ReservationDbContext : DbContext, IReservationDbContext
{
    public ReservationDbContext(DbContextOptions<ReservationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Hall> Halls => Set<Hall>();
    public DbSet<Seat> Seats => Set<Seat>();

    public Task<Hall?> GetHallAsync(int hallId, CancellationToken cancellationToken = default)
    {
        return Halls.FirstOrDefaultAsync(x => x.Id == hallId, cancellationToken);
    }

    public Task<Seat?> GetSeatAsync(int showtimeId, string seatNo, CancellationToken cancellationToken = default)
    {
        var normalizedSeatNo = seatNo.Trim().ToUpperInvariant();

        return Seats.FirstOrDefaultAsync(
            x => x.ShowtimeId == showtimeId && x.SeatNo == normalizedSeatNo,
            cancellationToken);
    }

    public Task<bool> ShowtimeSeatsExistAsync(int showtimeId, CancellationToken cancellationToken = default)
    {
        return Seats.AnyAsync(x => x.ShowtimeId == showtimeId, cancellationToken);
    }

    public void AddSeats(IEnumerable<Seat> seats)
    {
        Seats.AddRange(seats);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReservationDbContext).Assembly);
    }
}


// dotnet ef migrations add InitialCreateReservation \
//   --project src/Reservation/CinemaReservation.Reservation.Infrastructure \
//   --startup-project src/Reservation/CinemaReservation.Reservation.Api \
//   --output-dir Persistence/Migrations


//   dotnet ef database update \
//   --project src/Reservation/CinemaReservation.Reservation.Infrastructure \
//   --startup-project src/Reservation/CinemaReservation.Reservation.Api
