using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Common.Validation;
using CinemaReservation.Catalog.Application.Showtimes.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Showtimes.Commands.UpdateShowtime;

public sealed class UpdateShowtimeCommandHandler : IRequestHandler<UpdateShowtimeCommand, ShowtimeResponse?>
{
    private readonly ICatalogDbContext _dbContext;

    public UpdateShowtimeCommandHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShowtimeResponse?> Handle(UpdateShowtimeCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _dbContext.Showtimes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (showtime is null)
        {
            return null;
        }

        var movieExists = await _dbContext.Movies.AnyAsync(x => x.Id == request.MovieId, cancellationToken);
        if (!movieExists)
        {
            throw new ResourceNotFoundException("Movie");
        }

        showtime.MovieId = request.MovieId;
        showtime.Time = request.Time!.Value;
        showtime.Price = request.Price;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return new ShowtimeResponse(showtime.Id, showtime.MovieId, showtime.Time, showtime.Price);
    }
}
