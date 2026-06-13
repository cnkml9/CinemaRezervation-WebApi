using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Application.Movies.Commands.DeleteMovie;

public sealed record DeleteMovieCommand(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_ID")]
    int Id) : IRequest<bool>;
