using Microsoft.AspNetCore.Mvc.Rendering;

namespace TaskFlow.Web.ViewModels.TaskItems.Move;

public class MoveTaskItemViewModel
{
    public Guid Id { get; set; }

    public Guid BoardId { get; set; }

    public Guid BoardColumnId { get; set; }

    public int Order { get; set; }

    public List<SelectListItem> Columns { get; set; }
        = [];
}