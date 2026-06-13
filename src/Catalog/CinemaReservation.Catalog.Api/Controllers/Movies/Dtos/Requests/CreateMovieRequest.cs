using System.ComponentModel.DataAnnotations;
using CinemaReservation.Catalog.Domain.Enums;

namespace CinemaReservation.Catalog.Api.Controllers.Movies.Dtos.Requests;

public sealed class CreateMovieRequest
{
    [Required(ErrorMessage = "ERR_REQUIRED_TITLE")]
    [StringLength(200, MinimumLength = 2, ErrorMessage = "ERR_TITLE_LENGTH")]
    public string Title { get; init; } = string.Empty;

    [Range(1, 500, ErrorMessage = "ERR_DURATION_RANGE")]
    public int Duration { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_GENRE")]
    public MovieGenre Genre { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_STATUS")]
    public MovieStatus Status { get; init; }
}
