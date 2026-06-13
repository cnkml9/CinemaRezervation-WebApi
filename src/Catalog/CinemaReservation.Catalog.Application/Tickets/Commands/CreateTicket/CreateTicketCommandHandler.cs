using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Common.Validation;
using CinemaReservation.Catalog.Domain.Entities;
using CinemaReservation.Catalog.Domain.Enums;
using CinemaReservation.Common.Messaging.Events;
using MediatR;
using Microsoft.EntityFrameworkCore;
using MassTransit;

namespace CinemaReservation.Catalog.Application.Tickets.Commands.CreateTicket;

public sealed class CreateTicketCommandHandler : IRequestHandler<CreateTicketCommand, CreateTicketResult>
{
    private readonly ICatalogDbContext _dbContext;
    private readonly IPublishEndpoint _publishEndpoint;

    public CreateTicketCommandHandler(ICatalogDbContext dbContext, IPublishEndpoint publishEndpoint)
    {
        _dbContext = dbContext;
        _publishEndpoint = publishEndpoint;
    }

    public async Task<CreateTicketResult> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var showtimeExists = await _dbContext.Showtimes.AnyAsync(x => x.Id == request.ShowtimeId, cancellationToken);
        if (!showtimeExists)
        {
            throw new ResourceNotFoundException("Showtime");
        }

        var exists = await _dbContext.Tickets
            .AnyAsync(x => x.ShowtimeId == request.ShowtimeId && x.SeatNumber == request.SeatNumber && x.Status == TicketStatus.Purchased, cancellationToken);

        if (exists)
        {
            throw new InvalidOperationException("Bu koltuk bu seans için zaten satın alınmış.");
        }

        var ticket = new Ticket
        {
            ShowtimeId = request.ShowtimeId,
            UserId = request.UserId,
            SeatNumber = request.SeatNumber,
            Status = TicketStatus.Purchased
        };

        _dbContext.Tickets.Add(ticket);
        await _dbContext.SaveChangesAsync(cancellationToken);

        await _publishEndpoint.Publish(new TicketPurchasedEvent(
            ticket.Id,
            ticket.ShowtimeId,
            ticket.UserId,
            ticket.SeatNumber), cancellationToken);

        return new CreateTicketResult(
            ticket.Id,
            ticket.ShowtimeId,
            ticket.UserId,
            ticket.SeatNumber,
            ticket.Status.ToString());
    }
}
