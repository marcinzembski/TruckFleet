namespace FleetApi.Application.Trucks;

using FleetApi.Application.Common.Exceptions;
using FleetApi.Domain.Trucks;

public class TruckService
{
    private readonly ITruckRepository _repository;

    public TruckService(ITruckRepository repository) => _repository = repository;

    public async Task<TruckResponse> CreateAsync(
        string code, string name, string status, string? description, CancellationToken ct)
    {
        var truckCode = TruckCode.Create(code);

        if (await _repository.CodeExistsAsync(truckCode.Value, ct: ct))
            throw new ConflictException("Truck", "Code", code);

        var truck = Truck.Create(truckCode, name, Enum.Parse<TruckStatus>(status), description);
        await _repository.AddAsync(truck, ct);
        return TruckResponse.FromTruck(truck);
    }

    public async Task<TruckResponse> GetByIdAsync(Guid id, CancellationToken ct)
    {
        var truck = await _repository.GetByIdAsync(TruckId.From(id), ct)
            ?? throw new NotFoundException("Truck", id);
        return TruckResponse.FromTruck(truck);
    }

    public async Task<PagedTruckResponse> ListAsync(TruckFilter filter, CancellationToken ct)
    {
        var (items, total) = await _repository.GetPagedAsync(filter, ct);
        return new PagedTruckResponse(
            items.Select(TruckResponse.FromTruck).ToList(),
            total,
            filter.Page,
            filter.PageSize);
    }

    public async Task<TruckResponse> UpdateAsync(
        Guid id, string code, string name, string? description, CancellationToken ct)
    {
        var truckId = TruckId.From(id);
        var truck = await _repository.GetByIdAsync(truckId, ct)
            ?? throw new NotFoundException("Truck", id);

        var truckCode = TruckCode.Create(code);

        if (await _repository.CodeExistsAsync(truckCode.Value, truckId, ct))
            throw new ConflictException("Truck", "Code", code);

        truck.Update(truckCode, name, description);
        await _repository.UpdateAsync(truck, ct);
        return TruckResponse.FromTruck(truck);
    }

    public async Task<TruckResponse> ChangeStatusAsync(Guid id, string status, CancellationToken ct)
    {
        var truck = await _repository.GetByIdAsync(TruckId.From(id), ct)
            ?? throw new NotFoundException("Truck", id);

        var newStatus = Enum.Parse<TruckStatus>(status);
        if (truck.Status == newStatus)
            return TruckResponse.FromTruck(truck);

        truck.ChangeStatus(newStatus);
        await _repository.UpdateAsync(truck, ct);
        return TruckResponse.FromTruck(truck);
    }

    public async Task DeleteAsync(Guid id, CancellationToken ct)
    {
        await _repository.DeleteAsync(TruckId.From(id), ct);
    }
}
