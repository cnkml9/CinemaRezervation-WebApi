namespace CinemaReservation.Catalog.Application.Common.Validation;

public sealed class ResourceNotFoundException : Exception
{
    public string ResourceName { get; }

    public ResourceNotFoundException(string resourceName)
        : base($"{resourceName} not found")
    {
        ResourceName = resourceName;
    }
}
