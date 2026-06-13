using CinemaReservation.Catalog.Application.Tickets.Models;
using MediatR;

namespace CinemaReservation.Catalog.Application.Tickets.Queries.GetTickets;

public sealed record GetTicketsQuery() : IRequest<IReadOnlyList<TicketResponse>>;
