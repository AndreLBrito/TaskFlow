namespace TaskFlow.Api.Contracts.Boards;

public sealed record UpdateBoardRequest(
    string? Name,
    string? Description);
