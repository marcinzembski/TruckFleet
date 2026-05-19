namespace FleetApi.Api.Endpoints.Trucks.GetTruck;

using FleetApi.Application.Trucks;
using FastEndpoints;

public class GetTruckEndpoint : EndpointWithoutRequest<TruckResponse>
{
    private readonly TruckService _service;

    public GetTruckEndpoint(TruckService service) => _service = service;

    public override void Configure()
    {
        Get("/api/trucks/{id}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Trucks")
            .WithSummary("Get a truck by ID")
            .Produces<TruckResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await _service.GetByIdAsync(id, ct);
        await HttpContext.Response.SendAsync(result, cancellation: ct);
    }
}
