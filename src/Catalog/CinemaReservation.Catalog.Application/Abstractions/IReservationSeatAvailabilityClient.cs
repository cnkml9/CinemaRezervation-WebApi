namespace CinemaReservation.Catalog.Application.Abstractions;

public interface IReservationSeatAvailabilityClient
{
    Task<ReservationSeatAvailability> GetSeatAvailabilityAsync(
        int showtimeId,
        string seatNumber,
        CancellationToken cancellationToken = default);
}

public enum ReservationSeatAvailability
{
    Available,
    Reserved,
    SeatNotFound,
    ShowtimeSeatsNotFound
}
