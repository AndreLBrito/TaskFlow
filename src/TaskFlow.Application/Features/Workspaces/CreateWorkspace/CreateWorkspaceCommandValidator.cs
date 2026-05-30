using FluentValidation;

namespace TaskFlow.Application.Features.Workspaces.CreateWorkspace;

public class CreateWorkspaceCommandValidator
    : AbstractValidator<CreateWorkspaceCommand>
{
    public CreateWorkspaceCommandValidator()
    {
        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome do workspace é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do workspace não pode ter mais de 100 caracteres.");

        RuleFor(command => command.Description)
            .MaximumLength(500)
            .WithMessage("A descrição não pode ter mais de 500 caracteres.");
    }
}