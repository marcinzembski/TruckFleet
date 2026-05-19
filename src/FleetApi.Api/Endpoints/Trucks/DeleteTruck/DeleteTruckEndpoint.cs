namespace FleetApi.Api.Endpoints.Trucks.DeleteTruck;

using FleetApi.Application.Trucks;
using FastEndpoints;

public class DeleteTruckEndpoint : EndpointWithoutRequest
{
    private readonly TruckService _service;

    public DeleteTruckEndpoint(TruckService service) => _service = service;

    public override void Configure()
    {
        Delete("/api/trucks/{id}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Trucks")
            .WithSummary("Delete a truck")
            .Produces(StatusCodes.Status204NoContent));
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<Guid>("id");
        await _service.DeleteAsync(id, ct);
        HttpContext.Response.StatusCode = StatusCodes.Status204NoContent;
    }
}
