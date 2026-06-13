using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Showtimes.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Showtimes.Queries.GetShowtimes;

public sealed class GetShowtimesQueryHandler : IRequestHandler<GetShowtimesQuery, IReadOnlyList<ShowtimeResponse>>
{
    private readonly ICatalogDbContext _dbContext;

    public GetShowtimesQueryHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<ShowtimeResponse>> Handle(GetShowtimesQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Showtimes
            .OrderBy(x => x.Id)
            .Select(x => new ShowtimeResponse(x.Id, x.MovieId, x.Time, x.Price))
            .ToListAsync(cancellationToken);
    }
}
