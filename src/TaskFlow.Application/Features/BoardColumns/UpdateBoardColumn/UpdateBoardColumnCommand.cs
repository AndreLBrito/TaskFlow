using MediatR;

namespace TaskFlow.Application.Features.BoardColumns.UpdateBoardColumn;

public record UpdateBoardColumnCommand(
    Guid Id,
    string? Name) : IRequest;
