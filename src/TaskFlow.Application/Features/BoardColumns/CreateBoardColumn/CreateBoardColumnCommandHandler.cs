using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.BoardColumns.CreateBoardColumn;

public class CreateBoardColumnCommandHandler
    : IRequestHandler<CreateBoardColumnCommand, Guid>
{
    private readonly IBoardRepository _boardRepository;
    private readonly IBoardColumnRepository _boardColumnRepository;

    public CreateBoardColumnCommandHandler(
        IBoardRepository boardRepository,
        IBoardColumnRepository boardColumnRepository)
    {
        _boardRepository = boardRepository;
        _boardColumnRepository = boardColumnRepository;
    }

    public async Task<Guid> Handle(
        CreateBoardColumnCommand request,
        CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(
            request.BoardId,
            cancellationToken);

        if (board is null)
        {
            throw new NotFoundException("Quadro não encontrado.");
        }

        var order = await _boardColumnRepository.GetNextOrderAsync(
            request.BoardId,
            cancellationToken);

        var column = new BoardColumn(
            request.BoardId,
            request.Name!,
            order);

        await _boardColumnRepository.AddAsync(
            column,
            cancellationToken);

        return column.Id;
    }
}
