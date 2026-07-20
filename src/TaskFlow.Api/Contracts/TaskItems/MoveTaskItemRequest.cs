namespace TaskFlow.Api.Contracts.TaskItems;

public sealed record MoveTaskItemRequest(
    Guid TargetColumnId,
    int Order);
