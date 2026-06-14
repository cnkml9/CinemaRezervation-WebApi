using CinemaReservation.Catalog.Application.Showtimes.Models;
using CinemaReservation.Catalog.Application.Common.Validation;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Application.Showtimes.Commands.UpdateShowtime;

public sealed record UpdateShowtimeCommand(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_ID")]
    int Id,
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_MOVIE_ID")]
    int MovieId,
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_HALL_ID")]
    int HallId,
    [property: Required(ErrorMessage = "ERR_REQUIRED_TIME")]
    DateTime? Time,
    [property: DecimalRange(0.01, 999999.99, ErrorMessage = "ERR_PRICE_RANGE")]
    decimal Price) : IRequest<ShowtimeResponse?>;
