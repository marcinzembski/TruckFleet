namespace FleetApi.Api.Endpoints.Trucks.UpdateTruck;

using FleetApi.Application.Trucks;
using FastEndpoints;

public class UpdateTruckEndpoint : Endpoint<UpdateTruckRequest, TruckResponse>
{
    private readonly TruckService _service;

    public UpdateTruckEndpoint(TruckService service) => _service = service;

    public override void Configure()
    {
        Patch("/api/trucks/{id}");
        AllowAnonymous();
        Description(b => b
            .WithTags("Trucks")
            .WithSummary("Update truck data (name, code, description)")
            .Produces<TruckResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status409Conflict));
    }

    public override async Task HandleAsync(UpdateTruckRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await _service.UpdateAsync(id, req.Code, req.Name, req.Description, ct);
        await HttpContext.Response.SendAsync(result, cancellation: ct);
    }
}
