namespace CinemaReservation.Common.Messaging.Events;

public sealed record ShowtimeCreatedEvent(
    int ShowtimeId,
    int MovieId,
    int HallId,
    DateTime Time);
