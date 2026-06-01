using MediatR;

namespace TaskFlow.Application.Features.Boards.UpdateBoard;

public record UpdateBoardCommand(
    Guid Id,
    string? Name,
    string? Description
) : IRequest;