using CinemaReservation.Reservation.Application.Consumers;
using CinemaReservation.Reservation.Application.Abstractions;
using CinemaReservation.Reservation.Api.Middleware;
using CinemaReservation.Reservation.Infrastructure.Persistence;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

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

        return new BadRequestObjectResult(new
        {
            title = "Validation failed",
            status = StatusCodes.Status400BadRequest,
            errors
        });
    };
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TicketPurchasedConsumer>();
    x.AddConsumer<TicketCancelledConsumer>();
    x.AddConsumer<ShowtimeCreatedConsumer>();
    x.UsingRabbitMq((context, cfg) =>
    {
        var host = builder.Configuration["RabbitMQ:Host"] ?? "localhost";
        var port = ushort.Parse(builder.Configuration["RabbitMQ:Port"] ?? "5672");
        var username = builder.Configuration["RabbitMQ:Username"] ?? "guest";
        var password = builder.Configuration["RabbitMQ:Password"] ?? "guest";

        cfg.Host(host, port, "/", h =>
        {
            h.Username(username);
            h.Password(password);
        });

        cfg.ConfigureEndpoints(context);
    });
});
builder.Services.AddDbContext<ReservationDbContext>(options =>
{
    var connectionString = builder.Configuration.GetConnectionString("ReservationDb");
    options.UseNpgsql(connectionString);
});
builder.Services.AddScoped<IReservationDbContext>(sp => sp.GetRequiredService<ReservationDbContext>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<ReservationDbContext>();
    dbContext.Database.Migrate();
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseMiddleware<ExceptionHandlingMiddleware>();
app.MapControllers();

app.Run();
