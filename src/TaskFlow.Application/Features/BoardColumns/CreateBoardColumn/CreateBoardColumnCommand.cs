using MediatR;

namespace TaskFlow.Application.Features.BoardColumns.CreateBoardColumn;

public record CreateBoardColumnCommand(
    Guid BoardId,
    string? Name) : IRequest<Guid>;
