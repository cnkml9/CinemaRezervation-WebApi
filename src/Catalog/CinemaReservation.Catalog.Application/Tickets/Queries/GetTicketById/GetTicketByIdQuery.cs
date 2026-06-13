using CinemaReservation.Catalog.Application.Tickets.Models;
using MediatR;
using System.ComponentModel.DataAnnotations;

namespace CinemaReservation.Catalog.Application.Tickets.Queries.GetTicketById;

public sealed record GetTicketByIdQuery(
    [property: Range(1, int.MaxValue, ErrorMessage = "ERR_REQUIRED_ID")]
    int Id) : IRequest<TicketResponse?>;
