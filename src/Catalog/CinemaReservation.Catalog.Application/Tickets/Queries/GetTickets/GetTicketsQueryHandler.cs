using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Tickets.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Tickets.Queries.GetTickets;

public sealed class GetTicketsQueryHandler : IRequestHandler<GetTicketsQuery, IReadOnlyList<TicketResponse>>
{
    private readonly ICatalogDbContext _dbContext;

    public GetTicketsQueryHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyList<TicketResponse>> Handle(GetTicketsQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Tickets
            .OrderBy(x => x.Id)
            .Select(x => new TicketResponse(x.Id, x.ShowtimeId, x.UserId, x.SeatNumber, x.Status))
            .ToListAsync(cancellationToken);
    }
}
