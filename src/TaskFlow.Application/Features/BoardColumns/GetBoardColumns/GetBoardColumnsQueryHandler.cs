using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.BoardColumns.GetBoardColumns;

public class GetBoardColumnsQueryHandler
    : IRequestHandler<GetBoardColumnsQuery, IReadOnlyList<BoardColumnListItemDto>>
{
    private readonly IBoardColumnRepository _boardColumnRepository;

    public GetBoardColumnsQueryHandler(
        IBoardColumnRepository boardColumnRepository)
    {
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task<IReadOnlyList<BoardColumnListItemDto>> Handle(
        GetBoardColumnsQuery request,
        CancellationToken cancellationToken)
    {
        var columns = await _boardColumnRepository.GetByBoardIdAsync(
            request.BoardId,
            cancellationToken);

        return columns
            .Select(column => new BoardColumnListItemDto
            {
                Id = column.Id,
                BoardId = column.BoardId,
                Name = column.Name,
                Order = column.Order
            })
            .ToList();
    }
}
