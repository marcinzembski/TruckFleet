namespace FleetApi.Api.Endpoints.Trucks.CreateTruck;

public record CreateTruckRequest(
    string Code,
    string Name,
    string Status,
    string? Description);
