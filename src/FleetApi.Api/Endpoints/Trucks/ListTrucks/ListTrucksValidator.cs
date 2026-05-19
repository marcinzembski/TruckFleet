namespace FleetApi.Api.Endpoints.Trucks.ListTrucks;

using FleetApi.Domain.Trucks;
using FastEndpoints;
using FluentValidation;

public class ListTrucksValidator : Validator<ListTrucksRequest>
{
    private static readonly string[] ValidStatuses = Enum.GetNames<TruckStatus>();
    private static readonly string[] ValidSortFields = ["name", "code", "status", "createdat"];

    public ListTrucksValidator()
    {
        RuleForEach(x => x.Statuses)
            .Must(s => ValidStatuses.Contains(s))
            .WithMessage($"Each status must be one of: {string.Join(", ", ValidStatuses)}.")
            .When(x => x.Statuses is { Length: > 0 });

        RuleFor(x => x.SortBy)
            .Must(s => ValidSortFields.Contains(s.ToLowerInvariant()))
            .WithMessage($"SortBy must be one of: {string.Join(", ", ValidSortFields)}.")
            .When(x => x.SortBy is not null);

        RuleFor(x => x.SortDir)
            .Must(s => s == "asc" || s == "desc")
            .WithMessage("SortDir must be 'asc' or 'desc'.")
            .When(x => x.SortDir is not null);

        RuleFor(x => x.Page).GreaterThanOrEqualTo(1);
        RuleFor(x => x.PageSize).InclusiveBetween(1, 100);
    }
}
