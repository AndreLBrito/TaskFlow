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
    private readonly IBoardColumnRepository _boardColumnRepository;

    public CreateBoardCommandHandler(
        IWorkspaceRepository workspaceRepository,
        IBoardRepository boardRepository,
        IBoardColumnRepository boardColumnRepository)
    {
        _workspaceRepository = workspaceRepository;
        _boardRepository = boardRepository;
        _boardColumnRepository = boardColumnRepository;
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

        var defaultColumns = new List<BoardColumn>
        {
            new(
                board.Id,
                "A Fazer",
                1),

            new(
                board.Id,
                "Em Andamento",
                2),

            new(
                board.Id,
                "Concluído",
                3)
        };

        await _boardColumnRepository.AddRangeAsync(
            defaultColumns,
            cancellationToken);

        return board.Id;
    }
}