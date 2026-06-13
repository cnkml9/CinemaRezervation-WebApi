using CinemaReservation.Catalog.Application.Movies.Models;
using CinemaReservation.Catalog.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace CinemaReservation.Catalog.Application.Movies.Commands.CreateMovie;

public sealed record CreateMovieCommand(
    [property: Required(ErrorMessage = "ERR_REQUIRED_TITLE")]
    [property: StringLength(200, MinimumLength = 2, ErrorMessage = "ERR_TITLE_LENGTH")]
    string Title,
    [property: Range(1, 500, ErrorMessage = "ERR_DURATION_RANGE")]
    int Duration,
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_GENRE")]
    MovieGenre Genre,
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_STATUS")]
    MovieStatus Status) : IRequest<MovieResponse>;
