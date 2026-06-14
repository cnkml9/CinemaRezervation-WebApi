using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CinemaReservation.Reservation.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateRezervationTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Seats_HallId",
                table: "Seats",
                column: "HallId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Seats_HallId",
                table: "Seats");
        }
    }
}
