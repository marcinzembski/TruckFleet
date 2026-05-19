namespace FleetApi.Application.Trucks;

using FleetApi.Domain.Trucks;

public interface ITruckRepository
{
    Task<Truck?> GetByIdAsync(TruckId id, CancellationToken ct = default);
    Task<bool> CodeExistsAsync(string code, TruckId? excludeId = null, CancellationToken ct = default);
    Task<(IReadOnlyList<Truck> Items, int TotalCount)> GetPagedAsync(
        TruckFilter filter,
        CancellationToken ct = default);
    Task AddAsync(Truck truck, CancellationToken ct = default);
    Task UpdateAsync(Truck truck, CancellationToken ct = default);
    Task DeleteAsync(TruckId id, CancellationToken ct = default);
}
