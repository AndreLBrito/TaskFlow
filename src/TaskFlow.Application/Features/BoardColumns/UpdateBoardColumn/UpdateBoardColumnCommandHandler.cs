using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.BoardColumns.UpdateBoardColumn;

public class UpdateBoardColumnCommandHandler
    : IRequestHandler<UpdateBoardColumnCommand>
{
    private readonly IBoardColumnRepository _boardColumnRepository;

    public UpdateBoardColumnCommandHandler(
        IBoardColumnRepository boardColumnRepository)
    {
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task Handle(
        UpdateBoardColumnCommand request,
        CancellationToken cancellationToken)
    {
        var column = await _boardColumnRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (column is null)
        {
            throw new NotFoundException("Coluna não encontrada.");
        }

        column.Rename(request.Name!);

        await _boardColumnRepository.UpdateAsync(
            column,
            cancellationToken);
    }
}
