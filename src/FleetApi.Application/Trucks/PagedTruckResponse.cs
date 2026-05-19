namespace FleetApi.Application.Trucks;

public record PagedTruckResponse(
    IReadOnlyList<TruckResponse> Items,
    int TotalCount,
    int Page,
    int PageSize);
