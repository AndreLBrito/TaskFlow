using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.BoardColumns.GetBoardColumnById;

public class GetBoardColumnByIdQueryHandler
    : IRequestHandler<GetBoardColumnByIdQuery, BoardColumnDetailsDto?>
{
    private readonly IBoardColumnRepository _boardColumnRepository;

    public GetBoardColumnByIdQueryHandler(
        IBoardColumnRepository boardColumnRepository)
    {
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task<BoardColumnDetailsDto?> Handle(
        GetBoardColumnByIdQuery request,
        CancellationToken cancellationToken)
    {
        var column = await _boardColumnRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        return column is null
            ? null
            : new BoardColumnDetailsDto
            {
                Id = column.Id,
                BoardId = column.BoardId,
                Name = column.Name,
                Order = column.Order,
                CreatedAt = column.CreatedAt
            };
    }
}
