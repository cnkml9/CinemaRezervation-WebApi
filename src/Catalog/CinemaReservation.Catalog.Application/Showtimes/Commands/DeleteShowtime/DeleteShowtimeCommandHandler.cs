using CinemaReservation.Catalog.Application.Abstractions;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Showtimes.Commands.DeleteShowtime;

public sealed class DeleteShowtimeCommandHandler : IRequestHandler<DeleteShowtimeCommand, bool>
{
    private readonly ICatalogDbContext _dbContext;

    public DeleteShowtimeCommandHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<bool> Handle(DeleteShowtimeCommand request, CancellationToken cancellationToken)
    {
        var showtime = await _dbContext.Showtimes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);
        if (showtime is null)
        {
            return false;
        }

        _dbContext.Showtimes.Remove(showtime);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
