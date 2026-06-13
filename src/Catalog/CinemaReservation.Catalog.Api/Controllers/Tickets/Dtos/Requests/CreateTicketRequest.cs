using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Api.Controllers.Tickets.Dtos.Requests;

public sealed class CreateTicketRequest
{
    [Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_SHOWTIME_ID")]
    public int ShowtimeId { get; init; }

    [Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_USER_ID")]
    public int UserId { get; init; }

    [Required(ErrorMessage = "ERR_REQUIRED_SEAT_NUMBER")]
    [StringLength(20, MinimumLength = 1, ErrorMessage = "ERR_SEAT_NUMBER_LENGTH")]
    public string SeatNumber { get; init; } = string.Empty;
}
