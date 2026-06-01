using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Boards.UpdateBoard;

public class UpdateBoardCommandHandler
    : IRequestHandler<UpdateBoardCommand>
{
    private readonly IBoardRepository _boardRepository;

    public UpdateBoardCommandHandler(
        IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task Handle(
        UpdateBoardCommand request,
        CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (board is null)
        {
            throw new NotFoundException("Quadro não encontrado.");
        }

        board.Rename(request.Name!);
        board.UpdateDescription(request.Description);

        await _boardRepository.UpdateAsync(
            board,
            cancellationToken);
    }
}