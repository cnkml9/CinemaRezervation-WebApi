using CinemaReservation.Catalog.Domain.Common;
using CinemaReservation.Catalog.Domain.Enums;

namespace CinemaReservation.Catalog.Domain.Entities;

public class Movie : BaseEntity
{
    public string Title { get; set; } = string.Empty;
    public int Duration { get; set; }
    public MovieGenre Genre { get; set; }
    public MovieStatus Status { get; set; }

    public ICollection<Showtime> Showtimes { get; set; } = new List<Showtime>();
}
