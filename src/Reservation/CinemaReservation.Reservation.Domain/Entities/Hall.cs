using CinemaReservation.Reservation.Domain.Common;

namespace CinemaReservation.Reservation.Domain.Entities;

public class Hall : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public int TotalSeats { get; set; }

    public ICollection<Seat> Seats { get; set; } = new List<Seat>();
}
