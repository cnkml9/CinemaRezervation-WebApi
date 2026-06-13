using CinemaReservation.Reservation.Domain.Entities;

namespace CinemaReservation.Reservation.Application.Abstractions;

public interface IReservationDbContext
{
    Task<Seat?> GetSeatAsync(int showtimeId, string seatNo, CancellationToken cancellationToken = default);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
