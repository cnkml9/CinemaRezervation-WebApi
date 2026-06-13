using CinemaReservation.Catalog.Application.Common.Validation;

namespace CinemaReservation.Catalog.Api.Middleware;

public sealed class ExceptionHandlingMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (RequestValidationException ex)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                title = "Validation failed",
                status = StatusCodes.Status400BadRequest,
                errors = ex.Errors.Select(error => new
                {
                    field = error.Field,
                    code = error.Code,
                    message = error.Message
                })
            });
        }
        catch (ResourceNotFoundException ex)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            context.Response.ContentType = "application/json";

            await context.Response.WriteAsJsonAsync(new
            {
                title = "Resource not found",
                status = StatusCodes.Status404NotFound,
                resource = ex.ResourceName
            });
        }
    }
}
