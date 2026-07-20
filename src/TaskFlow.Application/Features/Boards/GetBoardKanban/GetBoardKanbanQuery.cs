using MediatR;

namespace TaskFlow.Application.Features.Boards.GetBoardKanban;

public record GetBoardKanbanQuery(Guid Id)
    : IRequest<KanbanBoardDto?>;
