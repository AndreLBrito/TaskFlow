using MediatR;

namespace TaskFlow.Application.Features.Boards.GetBoardById;

public record GetBoardByIdQuery(Guid Id)
    : IRequest<BoardDetailsDto?>;