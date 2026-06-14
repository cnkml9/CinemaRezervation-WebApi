using CinemaReservation.Reservation.Api.Controllers.Halls.Dtos.Requests;
using CinemaReservation.Reservation.Api.Controllers.Halls.Dtos.Responses;
using CinemaReservation.Reservation.Domain.Entities;
using CinemaReservation.Reservation.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Reservation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class HallsController : ControllerBase
{
    private readonly ReservationDbContext _dbContext;

    public HallsController(ReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<HallResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<HallResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var halls = await _dbContext.Halls
            .OrderBy(x => x.Id)
            .Select(x => new HallResponse(x.Id, x.Name, x.TotalSeats))
            .ToListAsync(cancellationToken);

        return Ok(halls);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(HallResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HallResponse>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var hall = await _dbContext.Halls
            .Where(x => x.Id == id)
            .Select(x => new HallResponse(x.Id, x.Name, x.TotalSeats))
            .FirstOrDefaultAsync(cancellationToken);

        return hall is null ? NotFound() : Ok(hall);
    }

    [HttpPost]
    [ProducesResponseType(typeof(HallResponse), StatusCodes.Status201Created)]
    public async Task<ActionResult<HallResponse>> Create([FromBody] CreateHallRequest request, CancellationToken cancellationToken)
    {
        var hall = new Hall
        {
            Name = request.Name.Trim(),
            TotalSeats = request.TotalSeats
        };

        _dbContext.Halls.Add(hall);
        await _dbContext.SaveChangesAsync(cancellationToken);

        var response = new HallResponse(hall.Id, hall.Name, hall.TotalSeats);
        return CreatedAtAction(nameof(GetById), new { id = hall.Id }, response);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(HallResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<HallResponse>> Update([FromRoute] int id, [FromBody] UpdateHallRequest request, CancellationToken cancellationToken)
    {
        var hall = await _dbContext.Halls.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (hall is null)
        {
            return NotFound();
        }

        hall.Name = request.Name.Trim();
        hall.TotalSeats = request.TotalSeats;

        await _dbContext.SaveChangesAsync(cancellationToken);

        return Ok(new HallResponse(hall.Id, hall.Name, hall.TotalSeats));
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var hall = await _dbContext.Halls.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        if (hall is null)
        {
            return NotFound();
        }

        _dbContext.Halls.Remove(hall);
        await _dbContext.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
