using CinemaReservation.Catalog.Domain.Common;

namespace CinemaReservation.Catalog.Domain.Entities;

public class Showtime : BaseEntity
{
    public int MovieId { get; set; }
    public DateTime Time { get; set; }
    public decimal Price { get; set; }

    public Movie Movie { get; set; } = null!;
    public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
}
