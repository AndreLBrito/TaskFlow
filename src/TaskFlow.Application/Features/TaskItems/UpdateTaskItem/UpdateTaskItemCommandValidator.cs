using FluentValidation;

namespace TaskFlow.Application.Features.TaskItems.UpdateTaskItem;

public class UpdateTaskItemCommandValidator
    : AbstractValidator<UpdateTaskItemCommand>
{
    public UpdateTaskItemCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty();

        RuleFor(command => command.Title)
            .NotEmpty()
            .WithMessage("O título é obrigatório.")
            .MaximumLength(150);

        RuleFor(command => command.Description)
            .MaximumLength(1000);
    }
}
