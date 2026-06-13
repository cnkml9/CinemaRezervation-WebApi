using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Application.Tickets.Commands.CancelTicket;

public sealed record CancelTicketCommand(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_ID")]
    int Id) : IRequest<bool>;
