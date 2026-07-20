namespace TaskFlow.Api.Contracts.Workspaces;

public sealed record UpdateWorkspaceRequest(
    string? Name,
    string? Description);
