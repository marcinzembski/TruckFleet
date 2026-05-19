namespace FleetApi.Api.Endpoints.Trucks.UpdateTruckStatus;

using FleetApi.Domain.Trucks;
using FastEndpoints;
using FluentValidation;

public class UpdateTruckStatusValidator : Validator<UpdateTruckStatusRequest>
{
    private static readonly string[] ValidStatuses = Enum.GetNames<TruckStatus>();

    public UpdateTruckStatusValidator()
    {
        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}.");
    }
}
