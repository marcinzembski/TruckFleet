namespace FleetApi.Application.Trucks;

using FleetApi.Domain.Trucks;

public record TruckFilter(
    string? Search = null,
    string? Name = null,
    string? Code = null,
    string? Description = null,
    TruckStatus[]? Statuses = null,
    string SortBy = "name",
    bool Ascending = true,
    int Page = 1,
    int PageSize = 20);
