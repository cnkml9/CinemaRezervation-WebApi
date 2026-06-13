using CinemaReservation.Catalog.Domain.Enums;

namespace CinemaReservation.Catalog.Application.Tickets.Models;

public sealed record TicketResponse(
    int Id,
    int ShowtimeId,
    int UserId,
    string SeatNumber,
    TicketStatus Status);
