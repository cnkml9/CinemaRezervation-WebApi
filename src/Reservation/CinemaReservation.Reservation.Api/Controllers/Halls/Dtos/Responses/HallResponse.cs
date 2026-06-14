namespace CinemaReservation.Reservation.Api.Controllers.Halls.Dtos.Responses;

public sealed record HallResponse(
    int Id,
    string Name,
    int TotalSeats);
