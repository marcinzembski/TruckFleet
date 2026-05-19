namespace FleetApi.Infrastructure.Persistence.Repositories;

using FleetApi.Application.Trucks;
using FleetApi.Domain.Trucks;
using Microsoft.EntityFrameworkCore;

public class TruckRepository : ITruckRepository
{
    private readonly AppDbContext _context;

    public TruckRepository(AppDbContext context) => _context = context;

    public async Task<Truck?> GetByIdAsync(TruckId id, CancellationToken ct = default) =>
        await _context.Trucks.FirstOrDefaultAsync(t => t.Id == id, ct);

    public async Task<bool> CodeExistsAsync(string code, TruckId? excludeId = null, CancellationToken ct = default) =>
        await _context.Trucks.AnyAsync(
            t => t.Code.Value == code && (excludeId == null || t.Id != excludeId), ct);

    public async Task<(IReadOnlyList<Truck> Items, int TotalCount)> GetPagedAsync(
        TruckFilter filter,
        CancellationToken ct = default)
    {
        var query = ApplyFilters(_context.Trucks.AsQueryable(), filter);
        query = ApplySort(query, filter);

        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((filter.Page - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ToListAsync(ct);

        return (items, total);
    }

    private static IQueryable<Truck> ApplyFilters(IQueryable<Truck> query, TruckFilter filter)
    {
        if (!string.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(t =>
                t.Name.Contains(filter.Search) ||
                t.Code.Value.Contains(filter.Search) ||
                (t.Description != null && t.Description.Contains(filter.Search)));

        if (!string.IsNullOrWhiteSpace(filter.Name))
            query = query.Where(t => t.Name.Contains(filter.Name));

        if (!string.IsNullOrWhiteSpace(filter.Code))
            query = query.Where(t => t.Code.Value.Contains(filter.Code));

        if (!string.IsNullOrWhiteSpace(filter.Description))
            query = query.Where(t => t.Description != null && t.Description.Contains(filter.Description));

        if (filter.Statuses is { Length: > 0 })
            query = query.Where(t => filter.Statuses.Contains(t.Status));

        return query;
    }

    private static IQueryable<Truck> ApplySort(IQueryable<Truck> query, TruckFilter filter) =>
        (filter.SortBy.ToLowerInvariant(), filter.Ascending) switch
        {
            ("code",      true)  => query.OrderBy(t => t.Code.Value),
            ("code",      false) => query.OrderByDescending(t => t.Code.Value),
            ("status",    true)  => query.OrderBy(t => t.Status),
            ("status",    false) => query.OrderByDescending(t => t.Status),
            ("createdat", true)  => query.OrderBy(t => t.CreatedAt),
            ("createdat", false) => query.OrderByDescending(t => t.CreatedAt),
            (_,           true)  => query.OrderBy(t => t.Name),
            (_,           false) => query.OrderByDescending(t => t.Name),
        };

    public async Task AddAsync(Truck truck, CancellationToken ct = default)
    {
        await _context.Trucks.AddAsync(truck, ct);
        await _context.SaveChangesAsync(ct);
    }

    public async Task UpdateAsync(Truck truck, CancellationToken ct = default)
    {
        _context.Trucks.Update(truck);
        await _context.SaveChangesAsync(ct);
    }

    public async Task DeleteAsync(TruckId id, CancellationToken ct = default)
    {
        var truck = await GetByIdAsync(id, ct);
        if (truck is null) return;
        _context.Trucks.Remove(truck);
        await _context.SaveChangesAsync(ct);
    }
}
