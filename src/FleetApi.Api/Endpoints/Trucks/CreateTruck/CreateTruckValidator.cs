namespace FleetApi.Api.Endpoints.Trucks.CreateTruck;

using FleetApi.Domain.Trucks;
using FastEndpoints;
using FluentValidation;

public class CreateTruckValidator : Validator<CreateTruckRequest>
{
    private static readonly string[] ValidStatuses = Enum.GetNames<TruckStatus>();

    public CreateTruckValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Status)
            .NotEmpty()
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"Status must be one of: {string.Join(", ", ValidStatuses)}.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null);
    }
}
