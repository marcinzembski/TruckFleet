namespace FleetApi.Application.Trucks;

using FleetApi.Domain.Trucks;

public record TruckResponse(
    Guid Id,
    string Code,
    string Name,
    string Status,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt)
{
    public static TruckResponse FromTruck(Truck t) =>
        new(t.Id.Value, t.Code.Value, t.Name, t.Status.ToString(), t.Description, t.CreatedAt, t.UpdatedAt);
}
