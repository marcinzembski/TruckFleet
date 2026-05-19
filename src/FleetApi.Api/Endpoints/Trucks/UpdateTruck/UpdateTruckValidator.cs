namespace FleetApi.Api.Endpoints.Trucks.UpdateTruck;

using FastEndpoints;
using FluentValidation;

public class UpdateTruckValidator : Validator<UpdateTruckRequest>
{
    public UpdateTruckValidator()
    {
        RuleFor(x => x.Code)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.Name)
            .NotEmpty()
            .MaximumLength(200);

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null);
    }
}
