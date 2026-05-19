namespace FleetApi.Api.Endpoints.Trucks.UpdateTruckStatus;

using FleetApi.Application.Trucks;
using FastEndpoints;

public class UpdateTruckStatusEndpoint : Endpoint<UpdateTruckStatusRequest, TruckResponse>
{
    private readonly TruckService _service;

    public UpdateTruckStatusEndpoint(TruckService service) => _service = service;

    public override void Configure()
    {
        Patch("/api/trucks/{id}/status");
        AllowAnonymous();
        Description(b => b
            .WithTags("Trucks")
            .WithSummary("Change truck status (validates transition rules)")
            .Produces<TruckResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .ProducesProblem(StatusCodes.Status422UnprocessableEntity));
    }

    public override async Task HandleAsync(UpdateTruckStatusRequest req, CancellationToken ct)
    {
        var id = Route<Guid>("id");
        var result = await _service.ChangeStatusAsync(id, req.Status, ct);
        await HttpContext.Response.SendAsync(result, cancellation: ct);
    }
}
