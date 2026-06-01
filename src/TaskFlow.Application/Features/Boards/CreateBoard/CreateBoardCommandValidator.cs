using FluentValidation;

namespace TaskFlow.Application.Features.Boards.CreateBoard;

public class CreateBoardCommandValidator
    : AbstractValidator<CreateBoardCommand>
{
    public CreateBoardCommandValidator()
    {
        RuleFor(command => command.WorkspaceId)
            .NotEmpty()
            .WithMessage("O workspace é obrigatório.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome do quadro é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome do quadro não pode ter mais de 100 caracteres.");

        RuleFor(command => command.Description)
            .MaximumLength(500)
            .WithMessage("A descrição não pode ter mais de 500 caracteres.");
    }
}