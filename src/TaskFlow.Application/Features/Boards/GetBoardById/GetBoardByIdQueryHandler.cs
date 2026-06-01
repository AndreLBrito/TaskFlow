using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Boards.GetBoardById;

public class GetBoardByIdQueryHandler
    : IRequestHandler<GetBoardByIdQuery, BoardDetailsDto?>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardByIdQueryHandler(
        IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<BoardDetailsDto?> Handle(
        GetBoardByIdQuery request,
        CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (board is null)
        {
            return null;
        }

        return new BoardDetailsDto
        {
            Id = board.Id,
            WorkspaceId = board.WorkspaceId,
            Name = board.Name,
            Description = board.Description,
            CreatedAt = board.CreatedAt
        };
    }
}