using MediatR;

namespace TaskFlow.Application.Features.Boards.CreateBoard;

public record CreateBoardCommand(
    Guid WorkspaceId,
    string? Name,
    string? Description
) : IRequest<Guid>;