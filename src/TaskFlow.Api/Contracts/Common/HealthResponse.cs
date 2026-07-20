namespace TaskFlow.Api.Contracts.Common;

public sealed record HealthResponse(
    string Status,
    string Application,
    DateTimeOffset Timestamp);
