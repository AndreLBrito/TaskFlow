using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Boards.GetBoards;

public class GetBoardsQueryHandler
    : IRequestHandler<GetBoardsQuery, IReadOnlyList<BoardListItemDto>>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardsQueryHandler(
        IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<IReadOnlyList<BoardListItemDto>> Handle(
        GetBoardsQuery request,
        CancellationToken cancellationToken)
    {
        var boards = await _boardRepository.GetByWorkspaceIdAsync(
            request.WorkspaceId,
            cancellationToken);

        return boards
            .Select(board => new BoardListItemDto
            {
                Id = board.Id,
                WorkspaceId = board.WorkspaceId,
                Name = board.Name,
                Description = board.Description,
                CreatedAt = board.CreatedAt
            })
            .ToList();
    }
}