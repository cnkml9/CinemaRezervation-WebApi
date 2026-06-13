using CinemaReservation.Catalog.Application.Movies.Commands.CreateMovie;
using CinemaReservation.Catalog.Application.Common.Behaviors;
using CinemaReservation.Catalog.Api.Middleware;
using CinemaReservation.Catalog.Infrastructure.Persistence;
using CinemaReservation.Common.Messaging;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(entry => entry.Value is { Errors.Count: > 0 })
            .SelectMany(entry => entry.Value!.Errors.Select(error => new
            {
                field = entry.Key,
                code = string.IsNullOrWhiteSpace(error.ErrorMessage)
                    ? $"ERR_INVALID_{entry.Key.Replace(".", "_").ToUpperInvariant()}"
                    : error.ErrorMessage,
                message = string.IsNullOrWhiteSpace(error.ErrorMessage) ? "Invalid value." : error.ErrorMessage
            }))
            .ToList();

        var problem = new
        {
            title = "Validation failed",
            status = StatusCodes.Status400BadRequest,
            errors
        };

        return new BadRequestObjectResult(problem);
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<CreateMovieCommand>());
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
builder.Services.AddRabbitMqMessaging(builder.Configuration);
builder.Services.AddDbContext<CatalogDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("CatalogDb");
    options.UseNpgsql(connectionString);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
