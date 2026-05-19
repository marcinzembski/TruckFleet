namespace FleetApi.Api.Endpoints.Trucks.ListTrucks;

using FleetApi.Application.Trucks;
using FleetApi.Domain.Trucks;
using FastEndpoints;

public class ListTrucksEndpoint : Endpoint<ListTrucksRequest, PagedTruckResponse>
{
    private readonly TruckService _service;

    public ListTrucksEndpoint(TruckService service) => _service = service;

    public override void Configure()
    {
        Get("/api/trucks");
        AllowAnonymous();
        Description(b => b
            .WithTags("Trucks")
            .WithSummary("List trucks with filtering and sorting")
            .Produces<PagedTruckResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest));
    }

    public override async Task HandleAsync(ListTrucksRequest req, CancellationToken ct)
    {
        var filter = new TruckFilter(
            Search: req.Search,
            Name: req.Name,
            Code: req.Code,
            Description: req.Description,
            Statuses: req.Statuses?.Select(Enum.Parse<TruckStatus>).ToArray(),
            SortBy: req.SortBy,
            Ascending: req.SortDir.Equals("asc", StringComparison.OrdinalIgnoreCase),
            Page: req.Page,
            PageSize: req.PageSize);

        var result = await _service.ListAsync(filter, ct);
        await HttpContext.Response.SendAsync(result, cancellation: ct);
    }
}
