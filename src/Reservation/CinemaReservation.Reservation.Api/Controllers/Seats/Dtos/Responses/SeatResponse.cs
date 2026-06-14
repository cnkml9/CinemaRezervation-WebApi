namespace CinemaReservation.Reservation.Api.Controllers.Seats.Dtos.Responses;

public sealed record SeatResponse(
    int Id,
    int HallId,
    int ShowtimeId,
    string SeatNo,
    bool IsReserved);
