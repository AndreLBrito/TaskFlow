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
            .MaximumLength(200)
            .WithMessage("O título da tarefa não pode ter mais de 200 caracteres.");

        RuleFor(command => command.Description)
            .MaximumLength(2000)
            .WithMessage("A descrição da tarefa não pode ter mais de 2000 caracteres.");
    }
}