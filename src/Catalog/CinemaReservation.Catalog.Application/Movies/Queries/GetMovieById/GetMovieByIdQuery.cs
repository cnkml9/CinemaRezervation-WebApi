using CinemaReservation.Catalog.Application.Movies.Models;
using System.ComponentModel.DataAnnotations;
using MediatR;

namespace CinemaReservation.Catalog.Application.Movies.Queries.GetMovieById;

public sealed record GetMovieByIdQuery(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_ID")]
    int Id) : IRequest<MovieResponse?>;
