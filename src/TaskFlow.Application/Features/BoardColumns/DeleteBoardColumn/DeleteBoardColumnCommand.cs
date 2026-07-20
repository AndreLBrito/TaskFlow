using MediatR;

namespace TaskFlow.Application.Features.BoardColumns.DeleteBoardColumn;

public record DeleteBoardColumnCommand(Guid Id) : IRequest;
