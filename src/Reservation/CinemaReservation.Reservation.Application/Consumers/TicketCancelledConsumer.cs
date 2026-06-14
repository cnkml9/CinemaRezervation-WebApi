using CinemaReservation.Common.Messaging.Events;
using CinemaReservation.Reservation.Application.Abstractions;
using MassTransit;

namespace CinemaReservation.Reservation.Application.Consumers;

public sealed class TicketCancelledConsumer : IConsumer<TicketCancelledEvent>
{
    private readonly IReservationDbContext _dbContext;

    public TicketCancelledConsumer(IReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<TicketCancelledEvent> context)
    {
        var seat = await _dbContext.GetSeatAsync(
            context.Message.ShowtimeId,
            context.Message.SeatNumber.Trim().ToUpperInvariant(),
            context.CancellationToken);

        if (seat is null)
        {
            return;
        }

        seat.IsReserved = false;
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }
}
