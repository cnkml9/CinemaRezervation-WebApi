namespace CinemaReservation.Catalog.Application.Common.Validation;

public sealed record ValidationFailureItem(
    string Field,
    string Code,
    string? Message = null);
