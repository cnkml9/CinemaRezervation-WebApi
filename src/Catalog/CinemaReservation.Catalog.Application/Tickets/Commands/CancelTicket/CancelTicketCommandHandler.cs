using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Common.Messaging.Events;
using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Tickets.Commands.CancelTicket;

public sealed class CancelTicketCommandHandler : IRequestHandler<CancelTicketCommand, bool>
{
    private readonly ICatalogDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public CancelTicketCommandHandler(ICatalogDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<bool> Handle(CancelTicketCommand request, CancellationToken cancellationToken)
    {
        var ticket = await _dbContext.Tickets.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (ticket is null)
        {
            return false;
        }

        _dbContext.Tickets.Remove(ticket);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new TicketCancelledEvent(
            ticket.Id,
            ticket.ShowtimeId,
            ticket.UserId,
            ticket.SeatNumber), cancellationToken);

        return true;
    }
}
