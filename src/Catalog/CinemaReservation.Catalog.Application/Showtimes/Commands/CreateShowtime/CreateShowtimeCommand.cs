using CinemaReservation.Catalog.Application.Showtimes.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Application.Showtimes.Commands.CreateShowtime;

public sealed record CreateShowtimeCommand(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_MOVIE_ID")]
    int MovieId,
    [property: Required(ErrorMessage = "ERR_REQUIRED_TIME")]
    DateTime? Time,
    [property: Range(typeof(decimal), "0.01", "999999.99", ErrorMessage = "ERR_PRICE_RANGE")]
    decimal Price) : IRequest<ShowtimeResponse>;
