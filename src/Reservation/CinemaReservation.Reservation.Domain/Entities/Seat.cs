using CinemaReservation.Reservation.Domain.Common;

namespace CinemaReservation.Reservation.Domain.Entities;

public class Seat : BaseEntity
{
    public int HallId { get; set; }
    public int ShowtimeId { get; set; }
    public string SeatNo { get; set; } = string.Empty;
    public bool IsReserved { get; set; }

    public Hall Hall { get; set; } = null!;
}
