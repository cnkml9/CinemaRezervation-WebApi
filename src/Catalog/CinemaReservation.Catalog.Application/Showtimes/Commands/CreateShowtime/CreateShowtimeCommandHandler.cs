using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Common.Validation;
using CinemaReservation.Catalog.Application.Showtimes.Models;
using CinemaReservation.Catalog.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Showtimes.Commands.CreateShowtime;

public sealed class CreateShowtimeCommandHandler : IRequestHandler<CreateShowtimeCommand, ShowtimeResponse>
{
    private readonly ICatalogDbContext _dbContext;

    public CreateShowtimeCommandHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShowtimeResponse> Handle(CreateShowtimeCommand request, CancellationToken cancellationToken)
    {
        var movieExists = await _dbContext.Movies.AnyAsync(x => x.Id == request.MovieId, cancellationToken);
        if (!movieExists)
        {
            throw new ResourceNotFoundException("Movie");
        }

        var showtime = new Showtime
        {
            MovieId = request.MovieId,
            Time = request.Time!.Value,
            Price = request.Price
        };

        _dbContext.Showtimes.Add(showtime);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ShowtimeResponse(showtime.Id, showtime.MovieId, showtime.Time, showtime.Price);
    }
}
