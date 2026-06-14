namespace CinemaReservation.Catalog.Application.Common.Validation;

public sealed class ExternalServiceUnavailableException : Exception
{
    public string ServiceName { get; }

    public ExternalServiceUnavailableException(string serviceName)
        : base($"{serviceName} service is unavailable")
    {
        ServiceName = serviceName;
    }
}
