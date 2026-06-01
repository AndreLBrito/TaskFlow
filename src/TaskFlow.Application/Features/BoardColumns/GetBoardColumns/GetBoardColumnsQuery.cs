using MediatR;

namespace TaskFlow.Application.Features.BoardColumns.GetBoardColumns;

public record GetBoardColumnsQuery(Guid BoardId)
    : IRequest<IReadOnlyList<BoardColumnListItemDto>>;