using Mapster;
using TaskFlow.Application.Features.Boards.CreateBoard;
using TaskFlow.Application.Features.Boards.GetBoardById;
using TaskFlow.Application.Features.Boards.GetBoards;
using TaskFlow.Application.Features.Boards.UpdateBoard;
using TaskFlow.Application.Features.TaskItems.CreateTaskItem;
using TaskFlow.Application.Features.TaskItems.GetTaskItemById;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;
using TaskFlow.Application.Features.Workspaces.UpdateWorkspace;
using TaskFlow.Web.ViewModels.Boards.Create;
using TaskFlow.Web.ViewModels.Boards.Details;
using TaskFlow.Web.ViewModels.Boards.List;
using TaskFlow.Web.ViewModels.Boards.Update;
using TaskFlow.Web.ViewModels.TaskItems.Create;
using TaskFlow.Web.ViewModels.TaskItems.Details;
using TaskFlow.Web.ViewModels.Workspaces.Create;
using TaskFlow.Web.ViewModels.Workspaces.Update;

namespace TaskFlow.Web.Mapping;

public static class MapsterConfiguration
{
    public static void RegisterMappings()
    {
        TypeAdapterConfig<CreateWorkspaceViewModel, CreateWorkspaceCommand>
            .NewConfig();

        TypeAdapterConfig<UpdateWorkspaceViewModel, UpdateWorkspaceCommand>
            .NewConfig();

        TypeAdapterConfig<WorkspaceDetailsDto, UpdateWorkspaceViewModel>
            .NewConfig();
        TypeAdapterConfig<CreateBoardViewModel, CreateBoardCommand>
            .NewConfig();

        TypeAdapterConfig<UpdateBoardViewModel, UpdateBoardCommand>
            .NewConfig();

        TypeAdapterConfig<BoardDetailsDto, BoardDetailsViewModel>
            .NewConfig();

        TypeAdapterConfig<BoardDetailsDto, UpdateBoardViewModel>
            .NewConfig();

        TypeAdapterConfig<BoardListItemDto, BoardListItemViewModel>
            .NewConfig();

        TypeAdapterConfig<CreateTaskItemViewModel, CreateTaskItemCommand>
            .NewConfig();

        TypeAdapterConfig<TaskItemDetailsDto, TaskItemDetailsViewModel>
            .NewConfig();
    }
}