using CinemaReservation.Catalog.Api.Controllers.Tickets.Dtos.Requests;
using CinemaReservation.Catalog.Application.Tickets.Commands.CancelTicket;
using CinemaReservation.Catalog.Application.Tickets.Commands.CreateTicket;
using CinemaReservation.Catalog.Application.Tickets.Models;
using CinemaReservation.Catalog.Application.Tickets.Queries.GetTicketById;
using CinemaReservation.Catalog.Application.Tickets.Queries.GetTickets;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaReservation.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class TicketsController : ControllerBase
{
    private readonly IMediator _mediator;

    public TicketsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<TicketResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<TicketResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var tickets = await _mediator.Send(new GetTicketsQuery(), cancellationToken);
        return Ok(tickets);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TicketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TicketResponse>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var ticket = await _mediator.Send(new GetTicketByIdQuery(id), cancellationToken);
        return ticket is null ? NotFound() : Ok(ticket);
    }

    [HttpPost]
    [ProducesResponseType(typeof(CreateTicketResult), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreateTicketResult>> Create([FromBody] CreateTicketRequest request, CancellationToken cancellationToken)
    {
        var ticket = await _mediator.Send(new CreateTicketCommand(request.ShowtimeId, request.UserId, request.SeatNumber), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = ticket.TicketId }, ticket);
    }

    [HttpPost("{id:int}/cancel")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Cancel([FromRoute] int id, CancellationToken cancellationToken)
    {
        var cancelled = await _mediator.Send(new CancelTicketCommand(id), cancellationToken);
        return cancelled ? NoContent() : NotFound();
    }
}
