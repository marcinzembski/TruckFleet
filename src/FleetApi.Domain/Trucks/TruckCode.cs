namespace FleetApi.Domain.Trucks;

using FleetApi.Domain.Exceptions;

public record TruckCode
{
    public string Value { get; }

    private TruckCode(string value) => Value = value;

    public static TruckCode Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new DomainException("Truck code cannot be empty.");
        if (value.Length > 50)
            throw new DomainException("Truck code cannot exceed 50 characters.");
        if (!value.All(char.IsAsciiLetterOrDigit))
            throw new DomainException("Truck code must be alphanumeric (letters and digits only).");

        return new TruckCode(value);
    }

    public override string ToString() => Value;
}
