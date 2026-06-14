using System.Net;
using System.Net.Http.Json;
using CinemaReservation.Catalog.Application.Abstractions;
using CinemaReservation.Catalog.Application.Common.Validation;

namespace CinemaReservation.Catalog.Api.Clients;

public sealed class ReservationSeatAvailabilityClient : IReservationSeatAvailabilityClient
{
    private readonly HttpClient _httpClient;

    public ReservationSeatAvailabilityClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ReservationSeatAvailability> GetSeatAvailabilityAsync(
        int showtimeId,
        string seatNumber,
        CancellationToken cancellationToken = default)
    {
        HttpResponseMessage response;

        try
        {
            response = await _httpClient.GetAsync($"/api/seats/showtime/{showtimeId}", cancellationToken);
        }
        catch (HttpRequestException)
        {
            throw new ExternalServiceUnavailableException("Reservation");
        }

        using (response)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return ReservationSeatAvailability.ShowtimeSeatsNotFound;
            }

            if (!response.IsSuccessStatusCode)
            {
                throw new ExternalServiceUnavailableException("Reservation");
            }

            var seats = await response.Content.ReadFromJsonAsync<List<ReservationSeatResponse>>(cancellationToken);
            var seat = seats?.FirstOrDefault(x =>
                string.Equals(x.SeatNo, seatNumber, StringComparison.OrdinalIgnoreCase));

            if (seat is null)
            {
                return ReservationSeatAvailability.SeatNotFound;
            }

            return seat.IsReserved
                ? ReservationSeatAvailability.Reserved
                : ReservationSeatAvailability.Available;
        }
    }

    private sealed record ReservationSeatResponse(
        int Id,
        int HallId,
        int ShowtimeId,
        string SeatNo,
        bool IsReserved);
}
