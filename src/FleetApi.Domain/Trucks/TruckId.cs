namespace FleetApi.Domain.Trucks;

public record TruckId(Guid Value)
{
    public static TruckId New() => new(Guid.NewGuid());
    public static TruckId From(Guid value) => new(value);
}
