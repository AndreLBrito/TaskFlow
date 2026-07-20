using MediatR;

namespace TaskFlow.Application.Features.BoardColumns.GetBoardColumnById;

public record GetBoardColumnByIdQuery(Guid Id)
    : IRequest<BoardColumnDetailsDto?>;
