namespace FleetApi.Application;

using FleetApi.Application.Trucks;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<TruckService>();
        return services;
    }
}
