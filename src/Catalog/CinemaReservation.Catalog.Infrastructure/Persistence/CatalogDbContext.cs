using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Catalog.Infrastructure.Persistence;

public class CatalogDbContext : DbContext, ICatalogDbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options)
        : base(options)
    {
    }

    public DbSet<Movie> Movies => Set<Movie>();
    public DbSet<Showtime> Showtimes => Set<Showtime>();
    public DbSet<Ticket> Tickets => Set<Ticket>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(CatalogDbContext).Assembly);
    }
}


// dotnet ef migrations add InitialCreate \
//   --project src/Catalog/CinemaReservation.Catalog.Infrastructure \
//   --startup-project src/Catalog/CinemaReservation.Catalog.Api \
//   --output-dir Persistence/Migrations


//   dotnet ef database update \
//   --project src/Catalog/CinemaReservation.Catalog.Infrastructure \
//   --startup-project src/Catalog/CinemaReservation.Catalog.Api