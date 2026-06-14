using System.ComponentModel.DataAnnotations;
using CinemaReservation.Catalog.Application.Common.Validation;

namespace CinemaReservation.Catalog.Api.Controllers.Showtimes.Dtos.Requests;

public sealed class UpdateShowtimeRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_MOVIE_ID")]
    public int MovieId { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_HALL_ID")]
    public int HallId { get; init; }

    [Required(ErrorMessage = "ERR_REQUIRED_TIME")]
    public DateTime? Time { get; init; }

    [DecimalRange(0.01, 999999.99, ErrorMessage = "ERR_PRICE_RANGE")]
    public decimal Price { get; init; }
}
