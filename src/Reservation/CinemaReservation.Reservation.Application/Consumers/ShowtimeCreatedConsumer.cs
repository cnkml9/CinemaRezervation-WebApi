using CinemaReservation.Common.Messaging.Events;
using CinemaReservation.Reservation.Application.Abstractions;
using CinemaReservation.Reservation.Domain.Entities;
using MassTransit;

namespace CinemaReservation.Reservation.Application.Consumers;

public sealed class ShowtimeCreatedConsumer : IConsumer<ShowtimeCreatedEvent>
{
    private const int SeatsPerRow = 10;
    private readonly IReservationDbContext _dbContext;

    public ShowtimeCreatedConsumer(IReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task Consume(ConsumeContext<ShowtimeCreatedEvent> context)
    {
        var message = context.Message;

        if (await _dbContext.ShowtimeSeatsExistAsync(message.ShowtimeId, context.CancellationToken))
        {
            return;
        }

        var hall = await _dbContext.GetHallAsync(message.HallId, context.CancellationToken);
        if (hall is null)
        {
            return;
        }

        var seats = Enumerable.Range(1, hall.TotalSeats)
            .Select(index => new Seat
            {
                HallId = hall.Id,
                ShowtimeId = message.ShowtimeId,
                SeatNo = CreateSeatNumber(index),
                IsReserved = false
            });

        _dbContext.AddSeats(seats);
        await _dbContext.SaveChangesAsync(context.CancellationToken);
    }

    private static string CreateSeatNumber(int index)
    {
        var rowIndex = (index - 1) / SeatsPerRow;
        var seatIndex = ((index - 1) % SeatsPerRow) + 1;
        var rowName = ((char)('A' + rowIndex)).ToString();

        return $"{rowName}{seatIndex}";
    }
}
