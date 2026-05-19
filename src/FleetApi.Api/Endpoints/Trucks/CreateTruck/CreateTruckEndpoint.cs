namespace FleetApi.Api.Endpoints.Trucks.CreateTruck;

using FleetApi.Application.Trucks;
using FastEndpoints;

public class CreateTruckEndpoint : Endpoint<CreateTruckRequest, TruckResponse>
{
    private readonly TruckService _service;

    public CreateTruckEndpoint(TruckService service) => _service = service;

    public override void Configure()
    {
        Post("/api/trucks");
        AllowAnonymous();
        Description(b => b
            .WithTags("Trucks")
            .WithSummary("Create a new truck")
            .Produces<TruckResponse>(StatusCodes.Status201Created)
            .ProducesProblem(StatusCodes.Status400BadRequest)
            .ProducesProblem(StatusCodes.Status409Conflict));
    }

    public override async Task HandleAsync(CreateTruckRequest req, CancellationToken ct)
    {
        var result = await _service.CreateAsync(req.Code, req.Name, req.Status, req.Description, ct);
        HttpContext.Response.Headers.Location = $"/api/trucks/{result.Id}";
        await HttpContext.Response.SendAsync(result, StatusCodes.Status201Created, cancellation: ct);
    }
}
