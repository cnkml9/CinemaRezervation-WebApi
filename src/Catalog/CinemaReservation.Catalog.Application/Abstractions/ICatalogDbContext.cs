using CinemaReservation.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Application.Abstractions;

public interface ICatalogDbContext
{
    DbSet<Movie> Movies { get; }
    DbSet<Showtime> Showtimes { get; }
    DbSet<Ticket> Tickets { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
