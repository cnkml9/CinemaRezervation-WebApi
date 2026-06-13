namespace CinemaReservation.Catalog.Application.Showtimes.Models;

public sealed record ShowtimeResponse(
    int Id,
    int MovieId,
    DateTime Time,
    decimal Price);
