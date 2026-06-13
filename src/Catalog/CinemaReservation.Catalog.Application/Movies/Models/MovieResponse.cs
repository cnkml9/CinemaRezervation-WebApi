using CinemaReservation.Catalog.Domain.Enums;

namespace CinemaReservation.Catalog.Application.Movies.Models;

public sealed record MovieResponse(
    int Id,
    string Title,
    int Duration,
    MovieGenre Genre,
    MovieStatus Status);
