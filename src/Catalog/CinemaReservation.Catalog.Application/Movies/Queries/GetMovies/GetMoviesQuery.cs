using CinemaReservation.Catalog.Application.Movies.Models;
using MediatR;

namespace CinemaReservation.Catalog.Application.Movies.Queries.GetMovies;

public sealed record GetMoviesQuery() : IRequest<IReadOnlyList<MovieResponse>>;
