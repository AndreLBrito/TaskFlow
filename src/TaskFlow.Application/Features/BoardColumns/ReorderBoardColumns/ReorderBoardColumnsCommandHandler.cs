using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.BoardColumns.ReorderBoardColumns;

public class ReorderBoardColumnsCommandHandler
    : IRequestHandler<ReorderBoardColumnsCommand>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardColumnRepository _boardColumnRepository;

    public ReorderBoardColumnsCommandHandler(
        IBoardRepository boardRepository,
        IBoardColumnRepository boardColumnRepository)
    {
        _boardRepository = boardRepository;
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task Handle(
        ReorderBoardColumnsCommand request,
        CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(
            request.BoardId,
            cancellationToken);

        if (board is null)
        {
            throw new NotFoundException("Quadro não encontrado.");
        }

        var columns = await _boardColumnRepository.GetByBoardIdAsync(
            request.BoardId,
            cancellationToken);

        var columnsById = columns.ToDictionary(column => column.Id);

        if (columns.Count != request.ColumnIds.Count ||
            request.ColumnIds.Any(id => !columnsById.ContainsKey(id)))
        {
            throw new BusinessRuleException(
                "A ordenação deve conter todas as colunas do quadro.");
        }

        for (var index = 0; index < request.ColumnIds.Count; index++)
        {
            columnsById[request.ColumnIds[index]].ChangeOrder(index);
        }

        await _boardColumnRepository.UpdateRangeAsync(
            columns,
            cancellationToken);
    }
}
