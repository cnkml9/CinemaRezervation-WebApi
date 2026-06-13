using CinemaReservation.Catalog.Api.Controllers.Showtimes.Dtos.Requests;
using CinemaReservation.Catalog.Application.Showtimes.Commands.CreateShowtime;
using CinemaReservation.Catalog.Application.Showtimes.Commands.DeleteShowtime;
using CinemaReservation.Catalog.Application.Showtimes.Commands.UpdateShowtime;
using CinemaReservation.Catalog.Application.Showtimes.Models;
using CinemaReservation.Catalog.Application.Showtimes.Queries.GetShowtimeById;
using CinemaReservation.Catalog.Application.Showtimes.Queries.GetShowtimes;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaReservation.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class ShowtimesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ShowtimesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<ShowtimeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<ShowtimeResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var showtimes = await _mediator.Send(new GetShowtimesQuery(), cancellationToken);
        return Ok(showtimes);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ShowtimeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShowtimeResponse>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var showtime = await _mediator.Send(new GetShowtimeByIdQuery(id), cancellationToken);
        return showtime is null ? NotFound() : Ok(showtime);
    }

    [HttpPost]
    [ProducesResponseType(typeof(ShowtimeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShowtimeResponse>> Create([FromBody] CreateShowtimeRequest request, CancellationToken cancellationToken)
    {
        var showtime = await _mediator.Send(new CreateShowtimeCommand(request.MovieId, request.Time, request.Price), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = showtime.Id }, showtime);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(ShowtimeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ShowtimeResponse>> Update([FromRoute] int id, [FromBody] UpdateShowtimeRequest request, CancellationToken cancellationToken)
    {
        var showtime = await _mediator.Send(new UpdateShowtimeCommand(id, request.MovieId, request.Time, request.Price), cancellationToken);
        return showtime is null ? NotFound() : Ok(showtime);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteShowtimeCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
