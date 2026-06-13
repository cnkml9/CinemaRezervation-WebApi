using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Movies.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Movies.Queries.GetMovies;

public sealed class GetMoviesQueryHandler : IRequestHandler<GetMoviesQuery, IReadOnlyList<MovieResponse>>
{
    private readonly ICatalogDbContext _dbContext;

    public GetMoviesQueryHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<MovieResponse>> Handle(GetMoviesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies
            .OrderBy(x => x.Id)
            .Select(x => new MovieResponse(x.Id, x.Title, x.Duration, x.Genre, x.Status))
            .ToListAsync(cancellationToken);
    }
}
