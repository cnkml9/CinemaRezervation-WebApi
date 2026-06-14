using CinemaReservation.Reservation.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservation.Reservation.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    [DbContext(typeof(ReservationDbContext))]
    [Migration("20260614120500_UpdateSeatUniqueness")]
    public partial class UpdateSeatUniqueness : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_HallId_ShowtimeId_SeatNo",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_ShowtimeId_SeatNo",
                table: "Seats",
                columns: new[] { "ShowtimeId", "SeatNo" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_ShowtimeId_SeatNo",
                table: "Seats");

            migrationBuilder.CreateIndex(
                name: "IX_Seats_HallId_ShowtimeId_SeatNo",
                table: "Seats",
                columns: new[] { "HallId", "ShowtimeId", "SeatNo" },
                unique: true);
        }
    }
}
