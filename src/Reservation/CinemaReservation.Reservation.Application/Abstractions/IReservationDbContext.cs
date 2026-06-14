using CinemaReservation.Reservation.Domain.Entities;

namespace CinemaReservation.Reservation.Application.Abstractions;

public interface IReservationDbContext
{
    Task<Hall?> GetHallAsync(int hallId, CancellationToken cancellationToken = default);

    Task<Seat?> GetSeatAsync(int showtimeId, string seatNo, CancellationToken cancellationToken = default);

    Task<bool> ShowtimeSeatsExistAsync(int showtimeId, CancellationToken cancellationToken = default);

    void AddSeats(IEnumerable<Seat> seats);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
