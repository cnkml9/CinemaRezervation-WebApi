using CinemaReservation.Reservation.Api.Controllers.Seats.Dtos.Responses;
using CinemaReservation.Reservation.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CinemaReservation.Reservation.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class SeatsController : ControllerBase
{
    private readonly ReservationDbContext _dbContext;

    public SeatsController(ReservationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    [HttpGet("showtime/{showtimeId:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<SeatResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<IReadOnlyList<SeatResponse>>> GetByShowtime(
        [FromRoute] int showtimeId,
        CancellationToken cancellationToken)
    {
        var seats = await _dbContext.Seats
            .Where(x => x.ShowtimeId == showtimeId)
            .OrderBy(x => x.SeatNo)
            .Select(x => new SeatResponse(x.Id, x.HallId, x.ShowtimeId, x.SeatNo, x.IsReserved))
            .ToListAsync(cancellationToken);

        return seats.Count == 0 ? NotFound() : Ok(seats);
    }
}
