using FluentValidation;

namespace TaskFlow.Application.Features.TaskItems.MoveTaskItem;

public class MoveTaskItemCommandValidator
    : AbstractValidator<MoveTaskItemCommand>
{
    public MoveTaskItemCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("A tarefa é obrigatória.");

        RuleFor(command => command.BoardColumnId)
            .NotEmpty()
            .WithMessage("A coluna é obrigatória.");

        RuleFor(command => command.Order)
            .GreaterThanOrEqualTo(0)
            .WithMessage("A ordem da tarefa não pode ser negativa.");
    }
}