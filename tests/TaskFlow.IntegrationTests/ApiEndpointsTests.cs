using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using TaskFlow.Api.Contracts.Common;
using TaskFlow.Application.Features.BoardColumns.GetBoardColumnById;
using TaskFlow.Application.Features.BoardColumns.GetBoardColumns;
using TaskFlow.Application.Features.Boards.GetBoardById;
using TaskFlow.Application.Features.Boards.GetBoardKanban;
using TaskFlow.Application.Features.Dashboard.GetDashboard;
using TaskFlow.Application.Features.Workspaces.GetWorkspaces;
using TaskFlow.Domain.Entities;

namespace TaskFlow.IntegrationTests;

public class ApiEndpointsTests : IClassFixture<TaskFlowApiFactory>
{
    private readonly TaskFlowApiFactory _factory;
    private readonly HttpClient _client;

    public ApiEndpointsTests(TaskFlowApiFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task WorkspaceAndBoardCrud_ShouldReturnExpectedStatuses()
    {
        await _factory.ResetDatabaseAsync();

        var workspaceResponse = await _client.PostAsJsonAsync(
            "/api/workspaces",
            new { name = "Produto", description = "Planejamento" });

        Assert.Equal(HttpStatusCode.Created, workspaceResponse.StatusCode);
        var workspace = await workspaceResponse.Content.ReadFromJsonAsync<IdResponse>();
        Assert.NotNull(workspace);

        var workspaces = await _client.GetFromJsonAsync<List<WorkspaceListItemDto>>(
            "/api/workspaces");
        Assert.Contains(workspaces!, item => item.Id == workspace.Id);

        var workspaceDetailsResponse = await _client.GetAsync(
            $"/api/workspaces/{workspace.Id}");
        Assert.Equal(HttpStatusCode.OK, workspaceDetailsResponse.StatusCode);

        var updateWorkspaceResponse = await _client.PutAsJsonAsync(
            $"/api/workspaces/{workspace.Id}",
            new { name = "Produto atualizado", description = "Planejamento" });
        Assert.Equal(HttpStatusCode.NoContent, updateWorkspaceResponse.StatusCode);

        var boardResponse = await _client.PostAsJsonAsync(
            $"/api/workspaces/{workspace.Id}/boards",
            new { name = "Roadmap", description = "Entrega" });

        Assert.Equal(HttpStatusCode.Created, boardResponse.StatusCode);
        var board = await boardResponse.Content.ReadFromJsonAsync<IdResponse>();
        Assert.NotNull(board);

        var boardDetails = await _client.GetFromJsonAsync<BoardDetailsDto>(
            $"/api/boards/{board.Id}");
        Assert.NotNull(boardDetails);
        Assert.Equal(workspace.Id, boardDetails.WorkspaceId);

        var boardsResponse = await _client.GetAsync(
            $"/api/workspaces/{workspace.Id}/boards");
        Assert.Equal(HttpStatusCode.OK, boardsResponse.StatusCode);

        var updateResponse = await _client.PutAsJsonAsync(
            $"/api/boards/{board.Id}",
            new { name = "Roadmap atualizado", description = "Entrega" });
        Assert.Equal(HttpStatusCode.NoContent, updateResponse.StatusCode);

        var deleteResponse = await _client.DeleteAsync($"/api/boards/{board.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteResponse.StatusCode);

        var deleteWorkspaceResponse = await _client.DeleteAsync(
            $"/api/workspaces/{workspace.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteWorkspaceResponse.StatusCode);
    }

    [Fact]
    public async Task ColumnsTasksAndKanban_ShouldSupportCrudAndMovement()
    {
        await _factory.ResetDatabaseAsync();
        var (workspace, board, firstColumn, secondColumn) = CreateBoardGraph();
        var firstTask = new TaskItem(firstColumn.Id, "Primeira", order: 0);
        var movedTask = new TaskItem(firstColumn.Id, "Mover", order: 1);

        await _factory.SeedAsync(
            workspace,
            board,
            firstColumn,
            secondColumn,
            firstTask,
            movedTask);

        var createColumnResponse = await _client.PostAsJsonAsync(
            $"/api/boards/{board.Id}/columns",
            new { name = "Revisão" });
        Assert.Equal(HttpStatusCode.Created, createColumnResponse.StatusCode);
        var createdColumn = await createColumnResponse.Content
            .ReadFromJsonAsync<IdResponse>();
        Assert.NotNull(createdColumn);

        var updateColumnResponse = await _client.PutAsJsonAsync(
            $"/api/columns/{createdColumn.Id}",
            new { name = "Em revisão" });
        Assert.Equal(HttpStatusCode.NoContent, updateColumnResponse.StatusCode);

        var columnDetails = await _client.GetFromJsonAsync<BoardColumnDetailsDto>(
            $"/api/columns/{createdColumn.Id}");
        Assert.NotNull(columnDetails);
        Assert.Equal("Em revisão", columnDetails.Name);

        var columns = await _client.GetFromJsonAsync<List<BoardColumnListItemDto>>(
            $"/api/boards/{board.Id}/columns");
        Assert.NotNull(columns);

        var reorderResponse = await _client.PutAsJsonAsync(
            $"/api/boards/{board.Id}/columns/order",
            new { columnIds = columns.Select(column => column.Id).Reverse() });
        Assert.Equal(HttpStatusCode.NoContent, reorderResponse.StatusCode);

        var createTaskResponse = await _client.PostAsJsonAsync(
            $"/api/columns/{firstColumn.Id}/tasks",
            new
            {
                title = "Criada pela API",
                description = "Teste de integração",
                dueDate = DateTime.UtcNow.Date.AddDays(2)
            });
        Assert.Equal(HttpStatusCode.Created, createTaskResponse.StatusCode);
        var createdTask = await createTaskResponse.Content.ReadFromJsonAsync<IdResponse>();
        Assert.NotNull(createdTask);

        var taskDetailsResponse = await _client.GetAsync(
            $"/api/tasks/{createdTask.Id}");
        Assert.Equal(HttpStatusCode.OK, taskDetailsResponse.StatusCode);

        var tasksByColumnResponse = await _client.GetAsync(
            $"/api/columns/{firstColumn.Id}/tasks");
        Assert.Equal(HttpStatusCode.OK, tasksByColumnResponse.StatusCode);

        var moveResponse = await _client.PutAsJsonAsync(
            $"/api/tasks/{movedTask.Id}/move",
            new { targetColumnId = secondColumn.Id, order = 0 });
        Assert.Equal(HttpStatusCode.NoContent, moveResponse.StatusCode);

        var kanban = await _client.GetFromJsonAsync<KanbanBoardDto>(
            $"/api/boards/{board.Id}/kanban");

        Assert.NotNull(kanban);
        var targetColumn = Assert.Single(
            kanban.Columns,
            column => column.Id == secondColumn.Id);
        var targetTask = Assert.Single(targetColumn.Tasks);
        Assert.Equal(movedTask.Id, targetTask.Id);
        Assert.Equal(0, targetTask.Order);

        var updateTaskResponse = await _client.PutAsJsonAsync(
            $"/api/tasks/{movedTask.Id}",
            new
            {
                title = "Movida",
                description = "Atualizada",
                dueDate = DateTime.UtcNow.Date.AddDays(1)
            });
        Assert.Equal(HttpStatusCode.NoContent, updateTaskResponse.StatusCode);

        var deleteTaskResponse = await _client.DeleteAsync(
            $"/api/tasks/{movedTask.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteTaskResponse.StatusCode);

        var deleteCreatedTaskResponse = await _client.DeleteAsync(
            $"/api/tasks/{createdTask.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteCreatedTaskResponse.StatusCode);

        var deleteColumnResponse = await _client.DeleteAsync(
            $"/api/columns/{createdColumn.Id}");
        Assert.Equal(HttpStatusCode.NoContent, deleteColumnResponse.StatusCode);
    }

    [Fact]
    public async Task Dashboard_ShouldReturnMvcIndicators()
    {
        await _factory.ResetDatabaseAsync();
        var (workspace, board, firstColumn, secondColumn) = CreateBoardGraph();
        var completedColumn = new BoardColumn(board.Id, "Concluído", 2);
        var openTask = new TaskItem(firstColumn.Id, "Aberta");
        var completedTask = new TaskItem(completedColumn.Id, "Concluída");

        await _factory.SeedAsync(
            workspace,
            board,
            firstColumn,
            secondColumn,
            completedColumn,
            openTask,
            completedTask);

        var dashboard = await _client.GetFromJsonAsync<DashboardDto>(
            "/api/dashboard");

        Assert.NotNull(dashboard);
        Assert.Equal(1, dashboard.WorkspacesCount);
        Assert.Equal(1, dashboard.BoardsCount);
        Assert.Equal(2, dashboard.TasksCount);
        Assert.Equal(1, dashboard.CompletedTasksCount);
    }

    [Fact]
    public async Task ValidationAndNotFound_ShouldReturnProblemDetails()
    {
        await _factory.ResetDatabaseAsync();

        var validationResponse = await _client.PostAsJsonAsync(
            "/api/workspaces",
            new { name = "", description = "" });
        Assert.Equal(HttpStatusCode.BadRequest, validationResponse.StatusCode);

        var validationProblem = await validationResponse.Content
            .ReadFromJsonAsync<JsonElement>();
        Assert.True(validationProblem.TryGetProperty("errors", out _));

        var notFoundResponse = await _client.DeleteAsync(
            $"/api/tasks/{Guid.NewGuid()}");
        Assert.Equal(HttpStatusCode.NotFound, notFoundResponse.StatusCode);

        var notFoundProblem = await notFoundResponse.Content
            .ReadFromJsonAsync<JsonElement>();
        Assert.Equal(404, notFoundProblem.GetProperty("status").GetInt32());

        var missingWorkspaceResponse = await _client.GetAsync(
            $"/api/workspaces/{Guid.NewGuid()}/boards");
        Assert.Equal(HttpStatusCode.NotFound, missingWorkspaceResponse.StatusCode);
    }

    [Fact]
    public async Task OpenApi_ShouldExposeAllAngularEndpoints()
    {
        var health = await _client.GetFromJsonAsync<HealthResponse>("/api/health");
        Assert.NotNull(health);
        Assert.Equal("healthy", health.Status);

        var document = await _client.GetFromJsonAsync<JsonElement>(
            "/openapi/v1.json");
        var paths = document.GetProperty("paths");

        var expectedOperations = new Dictionary<string, string[]>
        {
            ["/api/health"] = ["get"],
            ["/api/dashboard"] = ["get"],
            ["/api/workspaces"] = ["get", "post"],
            ["/api/workspaces/{id}"] = ["get", "put", "delete"],
            ["/api/workspaces/{workspaceId}/boards"] = ["get", "post"],
            ["/api/boards/{id}"] = ["get", "put", "delete"],
            ["/api/boards/{id}/kanban"] = ["get"],
            ["/api/boards/{boardId}/columns"] = ["get", "post"],
            ["/api/boards/{boardId}/columns/order"] = ["put"],
            ["/api/columns/{id}"] = ["get", "put", "delete"],
            ["/api/columns/{columnId}/tasks"] = ["get", "post"],
            ["/api/tasks/{id}"] = ["get", "put", "delete"],
            ["/api/tasks/{id}/move"] = ["put"]
        };

        foreach (var (path, methods) in expectedOperations)
        {
            Assert.True(
                paths.TryGetProperty(path, out var pathItem),
                $"Rota ausente: {path}");

            foreach (var method in methods)
            {
                Assert.True(
                    pathItem.TryGetProperty(method, out _),
                    $"Operação ausente: {method.ToUpperInvariant()} {path}");
            }
        }

        var deleteTaskResponses = paths
            .GetProperty("/api/tasks/{id}")
            .GetProperty("delete")
            .GetProperty("responses");
        Assert.True(deleteTaskResponses.TryGetProperty("204", out _));
        Assert.True(deleteTaskResponses.TryGetProperty("404", out _));

        var createBoardOperation = paths
            .GetProperty("/api/workspaces/{workspaceId}/boards")
            .GetProperty("post");
        Assert.True(createBoardOperation.TryGetProperty("requestBody", out _));
        Assert.Contains(
            createBoardOperation.GetProperty("parameters").EnumerateArray(),
            parameter =>
                parameter.GetProperty("name").GetString() == "workspaceId" &&
                parameter.GetProperty("in").GetString() == "path");

        var moveTaskOperation = paths
            .GetProperty("/api/tasks/{id}/move")
            .GetProperty("put");
        Assert.True(moveTaskOperation.TryGetProperty("requestBody", out _));
    }

    [Theory]
    [InlineData("http://localhost:4200")]
    [InlineData("https://localhost:4200")]
    public async Task Cors_ShouldAllowConfiguredAngularOrigins(string origin)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Options,
            "/api/workspaces");
        request.Headers.Add("Origin", origin);
        request.Headers.Add("Access-Control-Request-Method", "GET");

        using var response = await _client.SendAsync(request);

        Assert.Equal(HttpStatusCode.NoContent, response.StatusCode);
        Assert.Contains(
            origin,
            response.Headers.GetValues("Access-Control-Allow-Origin"));
    }

    private static (Workspace, Board, BoardColumn, BoardColumn) CreateBoardGraph()
    {
        var workspace = new Workspace("Workspace");
        var board = new Board(workspace.Id, "Quadro");
        var firstColumn = new BoardColumn(board.Id, "A Fazer", 0);
        var secondColumn = new BoardColumn(board.Id, "Em Andamento", 1);

        return (workspace, board, firstColumn, secondColumn);
    }
}
