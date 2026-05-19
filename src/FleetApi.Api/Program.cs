using FleetApi.Api.Infrastructure;
using FleetApi.Application;
using FleetApi.Infrastructure;
using FastEndpoints;
using FastEndpoints.Swagger;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddApplication()
    .AddInfrastructure()
    .AddExceptionHandler<GlobalExceptionHandler>()
    .AddProblemDetails()
    .AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "FleetApi";
            s.Version = "v1";
            s.Description = "REST API zbudowane na FastEndpoints, DDD i CQRS.";
        };
    });

var app = builder.Build();

app.UseExceptionHandler();

app.UseFastEndpoints(c =>
    c.Errors.UseProblemDetails())
   .UseSwaggerGen();

if (app.Environment.IsDevelopment())
    app.MapGet("/", () => Results.Redirect("/swagger")).ExcludeFromDescription();

app.Run();
