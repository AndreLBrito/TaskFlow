using FluentValidation;

namespace TaskFlow.Application.Features.Workspaces.UpdateWorkspace;

public class UpdateWorkspaceCommandValidator
    : AbstractValidator<UpdateWorkspaceCommand>
{
    public UpdateWorkspaceCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("O workspace é obrigatório.");

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