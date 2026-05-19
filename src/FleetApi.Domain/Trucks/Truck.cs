namespace FleetApi.Domain.Trucks;

using FleetApi.Domain.Common;
using FleetApi.Domain.Exceptions;

public class Truck : AggregateRoot<TruckId>
{
    public TruckCode Code { get; private set; } = null!;
    public string Name { get; private set; } = string.Empty;
    public TruckStatus Status { get; private set; }
    public string? Description { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    private Truck() { }

    private Truck(TruckId id, TruckCode code, string name, TruckStatus status, string? description)
        : base(id)
    {
        Code = code;
        Name = name;
        Status = status;
        Description = description;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }

    public static Truck Create(TruckCode code, string name, TruckStatus status, string? description)
        => new(TruckId.New(), code, name, status, description);

    public void Update(TruckCode code, string name, string? description)
    {
        Code = code;
        Name = name;
        Description = description;
        UpdatedAt = DateTime.UtcNow;
    }

    public void ChangeStatus(TruckStatus newStatus)
    {
        if (!IsValidTransition(Status, newStatus))
            throw new DomainException($"Cannot transition truck status from '{Status}' to '{newStatus}'.");

        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }

    private static bool IsValidTransition(TruckStatus from, TruckStatus to)
    {
        if (to == TruckStatus.OutOfService) return true;
        if (from == TruckStatus.OutOfService) return true;

        return (from, to) switch
        {
            (TruckStatus.Loading,   TruckStatus.ToJob)     => true,
            (TruckStatus.ToJob,     TruckStatus.AtJob)     => true,
            (TruckStatus.AtJob,     TruckStatus.Returning) => true,
            (TruckStatus.Returning, TruckStatus.Loading)   => true,
            _ => false
        };
    }
}
