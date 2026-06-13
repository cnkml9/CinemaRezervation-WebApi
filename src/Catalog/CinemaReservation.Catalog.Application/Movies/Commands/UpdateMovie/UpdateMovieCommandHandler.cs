using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Movies.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Movies.Commands.UpdateMovie;

public sealed class UpdateMovieCommandHandler : IRequestHandler<UpdateMovieCommand, MovieResponse?>
{
    private readonly ICatalogDbContext _dbContext;

    public UpdateMovieCommandHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<MovieResponse?> Handle(UpdateMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await _dbContext.Movies.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (movie is null)
        {
            return null;
        }

        movie.Title = request.Title.Trim();
        movie.Duration = request.Duration;
        movie.Genre = request.Genre;
        movie.Status = request.Status;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new MovieResponse(movie.Id, movie.Title, movie.Duration, movie.Genre, movie.Status);
    }
}
