using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Movies.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Movies.Queries.GetMovieById;

public sealed class GetMovieByIdQueryHandler : IRequestHandler<GetMovieByIdQuery, MovieResponse?>
{
    private readonly ICatalogDbContext _dbContext;

    public GetMovieByIdQueryHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MovieResponse?> Handle(GetMovieByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Movies
            .Where(x => x.Id == request.Id)
            .Select(x => new MovieResponse(x.Id, x.Title, x.Duration, x.Genre, x.Status))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
