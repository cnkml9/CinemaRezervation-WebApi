using CinemaReservation.Catalog.Application.Showtimes.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Application.Showtimes.Queries.GetShowtimeById;

public sealed record GetShowtimeByIdQuery(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_ID")]
    int Id) : IRequest<ShowtimeResponse?>;
