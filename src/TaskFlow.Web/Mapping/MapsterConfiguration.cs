using Mapster;
using TaskFlow.Application.Features.Workspaces.CreateWorkspace;
using TaskFlow.Application.Features.Workspaces.GetWorkspaceById;
using TaskFlow.Application.Features.Workspaces.UpdateWorkspace;
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
    }
}