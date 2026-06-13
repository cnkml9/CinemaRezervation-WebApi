using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Tickets.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Tickets.Queries.GetTicketById;

public sealed class GetTicketByIdQueryHandler : IRequestHandler<GetTicketByIdQuery, TicketResponse?>
{
    private readonly ICatalogDbContext _dbContext;

    public GetTicketByIdQueryHandler(ICatalogDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<TicketResponse?> Handle(GetTicketByIdQuery request, CancellationToken cancellationToken)
    {
        return await _dbContext.Tickets
            .Where(x => x.Id == request.Id)
            .Select(x => new TicketResponse(x.Id, x.ShowtimeId, x.UserId, x.SeatNumber, x.Status))
            .FirstOrDefaultAsync(cancellationToken);
    }
}
