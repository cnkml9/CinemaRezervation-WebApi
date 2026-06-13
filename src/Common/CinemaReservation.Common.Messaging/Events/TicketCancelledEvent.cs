namespace CinemaReservation.Common.Messaging.Events;

public sealed record TicketCancelledEvent(
    int TicketId,
    int ShowtimeId,
    int UserId,
    string SeatNumber);
