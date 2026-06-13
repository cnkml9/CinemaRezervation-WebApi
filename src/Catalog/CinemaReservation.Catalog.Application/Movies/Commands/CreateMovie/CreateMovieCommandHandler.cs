using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Movies.Models;
using CinemaReservation.Catalog.Domain.Entities;
using MediatR;

namespace CinemaReservation.Catalog.Application.Movies.Commands.CreateMovie;

public sealed class CreateMovieCommandHandler : IRequestHandler<CreateMovieCommand, MovieResponse>
{
    private readonly ICatalogDbContext _dbContext;

    public CreateMovieCommandHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MovieResponse> Handle(CreateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = new Movie
        {
            Title = request.Title.Trim(),
            Duration = request.Duration,
            Genre = request.Genre,
            Status = request.Status
        };

        _dbContext.Movies.Add(movie);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new MovieResponse(movie.Id, movie.Title, movie.Duration, movie.Genre, movie.Status);
    }
}
