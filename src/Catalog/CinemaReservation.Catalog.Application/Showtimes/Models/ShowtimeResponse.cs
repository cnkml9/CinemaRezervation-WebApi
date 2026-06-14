namespace CinemaReservation.Catalog.Application.Showtimes.Models;

public sealed record ShowtimeResponse(
    int Id,
    int MovieId,
    int HallId,
    DateTime Time,
    decimal Price);
