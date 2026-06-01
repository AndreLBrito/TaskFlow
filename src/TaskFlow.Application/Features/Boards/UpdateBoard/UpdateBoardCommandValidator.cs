using FluentValidation;

namespace TaskFlow.Application.Features.Boards.UpdateBoard;

public class UpdateBoardCommandValidator
    : AbstractValidator<UpdateBoardCommand>
{
    public UpdateBoardCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("O quadro é obrigatório.");

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