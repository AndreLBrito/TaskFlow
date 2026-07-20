using MediatR;

namespace TaskFlow.Application.Features.BoardColumns.ReorderBoardColumns;

public record ReorderBoardColumnsCommand(
    Guid BoardId,
    IReadOnlyList<Guid> ColumnIds) : IRequest;
