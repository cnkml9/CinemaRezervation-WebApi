namespace CinemaReservation.Catalog.Application.Common.Validation;

public sealed class RequestValidationException : Exception
{
    public IReadOnlyCollection<ValidationFailureItem> Errors { get; }

    public RequestValidationException(IEnumerable<ValidationFailureItem> errors)
        : base("Validation failed")
    {
        Errors = errors.ToArray();
    }
}
