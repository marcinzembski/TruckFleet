namespace FleetApi.Api.Endpoints.Trucks.UpdateTruck;

public record UpdateTruckRequest(
    string Code,
    string Name,
    string? Description);
