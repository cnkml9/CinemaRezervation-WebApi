using CinemaReservation.Common.Messaging.Events;
using CinemaReservation.Reservation.Application.Abstractions;
using MassTransit;

namespace CinemaReservation.Reservation.Application.Consumers;

public sealed class TicketPurchasedConsumer : IConsumer<TicketPurchasedEvent>
{
    private readonly IReservationDbContext _dbContext;

    public TicketPurchasedConsumer(IReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<TicketPurchasedEvent> context)
    {
        var seat = await _dbContext.GetSeatAsync(
            context.Message.ShowtimeId,
            context.Message.SeatNumber.Trim().ToUpperInvariant(),
            context.CancellationToken);

        if (seat is null)
        {
            return;
        }

        seat.IsReserved = true;
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
