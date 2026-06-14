using CinemaReservation.Catalog.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservation.Catalog.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(CatalogDbContext))]
    [Migration("20260614120000_AddHallIdToShowtimes")]
    public partial class AddHallIdToShowtimes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "HallId",
                table: "Showtimes",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Showtimes_HallId_Time",
                table: "Showtimes",
                columns: new[] { "HallId", "Time" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Showtimes_HallId_Time",
                table: "Showtimes");

            migrationBuilder.DropColumn(
                name: "HallId",
                table: "Showtimes");
        }
    }
}
