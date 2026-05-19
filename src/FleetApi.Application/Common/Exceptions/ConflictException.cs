namespace FleetApi.Application.Common.Exceptions;

public class ConflictException : Exception
{
    public ConflictException(string entity, string field, object value)
        : base($"{entity} with {field} '{value}' already exists.") { }
}
