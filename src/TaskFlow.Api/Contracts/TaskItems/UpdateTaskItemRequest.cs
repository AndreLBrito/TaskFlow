namespace TaskFlow.Api.Contracts.TaskItems;

public sealed record UpdateTaskItemRequest(
    string? Title,
    string? Description,
    DateTime? DueDate);
