using FluentValidation;

namespace TaskFlow.Application.Features.BoardColumns.UpdateBoardColumn;

public class UpdateBoardColumnCommandValidator
    : AbstractValidator<UpdateBoardColumnCommand>
{
    public UpdateBoardColumnCommandValidator()
    {
        RuleFor(command => command.Id)
            .NotEmpty()
            .WithMessage("A coluna é obrigatória.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome da coluna é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome da coluna não pode ter mais de 100 caracteres.");
    }
}
