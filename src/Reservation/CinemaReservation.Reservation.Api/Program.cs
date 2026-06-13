using CinemaReservation.Reservation.Application.Consumers;
using CinemaReservation.Reservation.Application.Abstractions;
using CinemaReservation.Reservation.Infrastructure.Persistence;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<TicketPurchasedConsumer>();
    x.AddConsumer<TicketCancelledConsumer>();
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
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.Run();
