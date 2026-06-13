using CinemaReservation.Catalog.Api.Controllers.Movies.Dtos.Requests;
using CinemaReservation.Catalog.Application.Movies.Commands.CreateMovie;
using CinemaReservation.Catalog.Application.Movies.Commands.DeleteMovie;
using CinemaReservation.Catalog.Application.Movies.Commands.UpdateMovie;
using CinemaReservation.Catalog.Application.Movies.Models;
using CinemaReservation.Catalog.Application.Movies.Queries.GetMovieById;
using CinemaReservation.Catalog.Application.Movies.Queries.GetMovies;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CinemaReservation.Catalog.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class MoviesController : ControllerBase
{
    private readonly IMediator _mediator;

    public MoviesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<MovieResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<MovieResponse>>> GetAll(CancellationToken cancellationToken)
    {
        var movies = await _mediator.Send(new GetMoviesQuery(), cancellationToken);
        return Ok(movies);
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieResponse>> GetById([FromRoute] int id, CancellationToken cancellationToken)
    {
        var movie = await _mediator.Send(new GetMovieByIdQuery(id), cancellationToken);
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpPost]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<MovieResponse>> Create([FromBody] CreateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await _mediator.Send(new CreateMovieCommand(request.Title, request.Duration, request.Genre, request.Status), cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = movie.Id }, movie);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(MovieResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<MovieResponse>> Update([FromRoute] int id, [FromBody] UpdateMovieRequest request, CancellationToken cancellationToken)
    {
        var movie = await _mediator.Send(new UpdateMovieCommand(id, request.Title, request.Duration, request.Genre, request.Status), cancellationToken);
        return movie is null ? NotFound() : Ok(movie);
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id, CancellationToken cancellationToken)
    {
        var deleted = await _mediator.Send(new DeleteMovieCommand(id), cancellationToken);
        return deleted ? NoContent() : NotFound();
    }
}
