using CinemaReservation.Catalog.Application.Showtimes.Models;
using MediatR;

namespace CinemaReservation.Catalog.Application.Showtimes.Queries.GetShowtimes;

public sealed record GetShowtimesQuery() : IRequest<IReadOnlyList<ShowtimeResponse>>;
