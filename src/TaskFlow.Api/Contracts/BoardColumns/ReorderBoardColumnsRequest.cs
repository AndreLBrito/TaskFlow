namespace TaskFlow.Api.Contracts.BoardColumns;

public sealed record ReorderBoardColumnsRequest(
    IReadOnlyList<Guid> ColumnIds);
