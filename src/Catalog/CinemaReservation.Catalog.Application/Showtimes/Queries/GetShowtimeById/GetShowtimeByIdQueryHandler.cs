using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Showtimes.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Showtimes.Queries.GetShowtimeById;

public sealed class GetShowtimeByIdQueryHandler : IRequestHandler<GetShowtimeByIdQuery, ShowtimeResponse?>
{
    private readonly ICatalogDbContext _dbContext;

    public GetShowtimeByIdQueryHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ShowtimeResponse?> Handle(GetShowtimeByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Showtimes
            .Where(x => x.Id == request.Id)
            .Select(x => new ShowtimeResponse(x.Id, x.MovieId, x.HallId, x.Time, x.Price))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
