using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Boards.DeleteBoard;

public class DeleteBoardCommandHandler
    : IRequestHandler<DeleteBoardCommand>
{
    private readonly IBoardRepository _boardRepository;

    public DeleteBoardCommandHandler(
        IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task Handle(
        DeleteBoardCommand request,
        CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdAsync(
            request.Id,
            cancellationToken);

        if (board is null)
        {
            throw new NotFoundException("Quadro não encontrado.");
        }

        await _boardRepository.DeleteAsync(
            board,
            cancellationToken);
    }
}