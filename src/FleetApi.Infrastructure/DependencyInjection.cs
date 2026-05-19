namespace FleetApi.Infrastructure;

using FleetApi.Application.Trucks;
using FleetApi.Infrastructure.Persistence;
using FleetApi.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<AppDbContext>(opts =>
            opts.UseInMemoryDatabase("FleetApiDb"));

        services.AddScoped<ITruckRepository, TruckRepository>();

        return services;
    }
}
