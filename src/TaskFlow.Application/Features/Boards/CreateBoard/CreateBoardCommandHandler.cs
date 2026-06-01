using MediatR;
using TaskFlow.Application.Common.Exceptions;
using TaskFlow.Application.Interfaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.Application.Features.Boards.CreateBoard;

public class CreateBoardCommandHandler
    : IRequestHandler<CreateBoardCommand, Guid>
{
    private readonly IWorkspaceRepository _workspaceRepository;
    private readonly IBoardRepository _boardRepository;

    public CreateBoardCommandHandler(
        IWorkspaceRepository workspaceRepository,
        IBoardRepository boardRepository)
    {
        _workspaceRepository = workspaceRepository;
        _boardRepository = boardRepository;
    }

    public async Task<Guid> Handle(
        CreateBoardCommand request,
        CancellationToken cancellationToken)
    {
        var workspace = await _workspaceRepository.GetByIdAsync(
            request.WorkspaceId,
            cancellationToken);

        if (workspace is null)
        {
            throw new NotFoundException("Workspace não encontrado.");
        }

        var board = new Board(
            request.WorkspaceId,
            request.Name!,
            request.Description);

        await _boardRepository.AddAsync(
            board,
            cancellationToken);

        return board.Id;
    }
}