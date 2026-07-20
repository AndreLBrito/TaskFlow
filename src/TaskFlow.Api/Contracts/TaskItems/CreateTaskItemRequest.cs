namespace TaskFlow.Api.Contracts.TaskItems;

public sealed record CreateTaskItemRequest(
    string? Title,
    string? Description,
    DateTime? DueDate);
