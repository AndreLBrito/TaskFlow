using MediatR;
using TaskFlow.Application.Interfaces;

namespace TaskFlow.Application.Features.Boards.GetBoardKanban;

public class GetBoardKanbanQueryHandler
    : IRequestHandler<GetBoardKanbanQuery, KanbanBoardDto?>
{
    private readonly IBoardRepository _boardRepository;

    public GetBoardKanbanQueryHandler(IBoardRepository boardRepository)
    {
        _boardRepository = boardRepository;
    }

    public async Task<KanbanBoardDto?> Handle(
        GetBoardKanbanQuery request,
        CancellationToken cancellationToken)
    {
        var board = await _boardRepository.GetByIdWithColumnsAndTasksAsync(
            request.Id,
            cancellationToken);

        if (board is null)
        {
            return null;
        }

        return new KanbanBoardDto
        {
            Id = board.Id,
            WorkspaceId = board.WorkspaceId,
            Name = board.Name,
            Description = board.Description,
            Columns = board.Columns
                .OrderBy(column => column.Order)
                .Select(column => new KanbanColumnDto
                {
                    Id = column.Id,
                    Name = column.Name,
                    Order = column.Order,
                    Tasks = column.Tasks
                        .OrderBy(task => task.Order)
                        .Select(task => new KanbanTaskItemDto
                        {
                            Id = task.Id,
                            Title = task.Title,
                            Description = task.Description,
                            DueDate = task.DueDate,
                            Order = task.Order
                        })
                        .ToList()
                })
                .ToList()
        };
    }
}
