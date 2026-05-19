namespace FleetApi.Api.Endpoints.Trucks.ListTrucks;

public class ListTrucksRequest
{
    public string? Search { get; set; }
    public string? Name { get; set; }
    public string? Code { get; set; }
    public string? Description { get; set; }
    public string[]? Statuses { get; set; }
    public string SortBy { get; set; } = "name";
    public string SortDir { get; set; } = "asc";
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
}
