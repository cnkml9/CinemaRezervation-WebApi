using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Reservation.Api.Controllers.Halls.Dtos.Requests;

public sealed class CreateHallRequest
{
    [Required(ErrorMessage = "ERR_REQUIRED_NAME")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "ERR_NAME_LENGTH")]
    public string Name { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_TOTAL_SEATS")]
    public int TotalSeats { get; init; }
}
