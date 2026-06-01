using MediatR;

namespace TaskFlow.Application.Features.Boards.DeleteBoard;

public record DeleteBoardCommand(Guid Id) : IRequest;