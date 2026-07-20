using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.BoardColumns.DeleteBoardColumn;

public class DeleteBoardColumnCommandHandler
    : IRequestHandler<DeleteBoardColumnCommand>
{
    private readonly IBoardColumnRepository _boardColumnRepository;

    public DeleteBoardColumnCommandHandler(
        IBoardColumnRepository boardColumnRepository)
    {
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task Handle(
        DeleteBoardColumnCommand request,
        CancellationToken cancellationToken)
    {
        var column = await _boardColumnRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (column is null)
        {
            throw new NotFoundException("Coluna não encontrada.");
        }

        await _boardColumnRepository.DeleteAsync(
            column,
            cancellationToken);
    }
}
