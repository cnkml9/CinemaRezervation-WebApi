using CinemaReservation.Catalog.Domain.Common;
using CinemaReservation.Catalog.Domain.Enums;

namespace CinemaReservation.Catalog.Domain.Entities;

public class Ticket : BaseEntity
{
    public int ShowtimeId { get; set; }
    public int UserId { get; set; }
    public string SeatNumber { get; set; } = string.Empty;
    public TicketStatus Status { get; set; }

    public Showtime Showtime { get; set; } = null!;
}
