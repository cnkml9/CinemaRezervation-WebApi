namespace CinemaReservation.Common.Messaging.Events;

public sealed record TicketPurchasedEvent(
    int TicketId,
    int ShowtimeId,
    int UserId,
    string SeatNumber);
