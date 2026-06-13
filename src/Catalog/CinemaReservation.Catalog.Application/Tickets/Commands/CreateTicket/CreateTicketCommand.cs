using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Application.Tickets.Commands.CreateTicket;

public sealed record CreateTicketCommand(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_SHOWTIME_ID")]
    int ShowtimeId,
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_USER_ID")]
    int UserId,
    [property: Required(ErrorMessage = "ERR_REQUIRED_SEAT_NUMBER")]
    [property: StringLength(20, MinimumLength = 1, ErrorMessage = "ERR_SEAT_NUMBER_LENGTH")]
    string SeatNumber) : IRequest<CreateTicketResult>;

public sealed record CreateTicketResult(
    int TicketId,
    int ShowtimeId,
    int UserId,
    string SeatNumber,
    string Status);
