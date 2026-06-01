using MediatR;

namespace TaskFlow.Application.Features.Boards.GetBoards;

public record GetBoardsQuery(Guid WorkspaceId)
    : IRequest<IReadOnlyList<BoardListItemDto>>;