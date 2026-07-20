using FluentValidation;

namespace TaskFlow.Application.Features.TaskItems.CreateTaskItem;

public class CreateTaskItemCommandValidator
    : AbstractValidator<CreateTaskItemCommand>
{
    public CreateTaskItemCommandValidator()
    {
        RuleFor(command => command.BoardColumnId)
            .NotEmpty()
            .WithMessage("A coluna é obrigatória.");

        RuleFor(command => command.Title)
            .NotEmpty()
            .WithMessage("O título da tarefa é obrigatório.")
            .MaximumLength(150)
            .WithMessage("O título da tarefa não pode ter mais de 150 caracteres.");

        RuleFor(command => command.Description)
            .MaximumLength(1000)
            .WithMessage("A descrição da tarefa não pode ter mais de 1000 caracteres.");
    }
}
