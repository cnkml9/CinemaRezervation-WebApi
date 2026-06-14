using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace CinemaReservation.Catalog.Application.Common.Validation;

public sealed class DecimalRangeAttribute : ValidationAttribute
{
    private readonly decimal _minimum;
    private readonly decimal _maximum;

    public DecimalRangeAttribute(double minimum, double maximum)
    {
        _minimum = Convert.ToDecimal(minimum, CultureInfo.InvariantCulture);
        _maximum = Convert.ToDecimal(maximum, CultureInfo.InvariantCulture);
    }

    public override bool IsValid(object? value)
    {
        if (value is null)
        {
            return true;
        }

        var decimalValue = Convert.ToDecimal(value, CultureInfo.InvariantCulture);
        return decimalValue >= _minimum && decimalValue <= _maximum;
    }
}
