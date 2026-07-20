using FluentValidation;

namespace TaskFlow.Application.Features.BoardColumns.CreateBoardColumn;

public class CreateBoardColumnCommandValidator
    : AbstractValidator<CreateBoardColumnCommand>
{
    public CreateBoardColumnCommandValidator()
    {
        RuleFor(command => command.BoardId)
            .NotEmpty()
            .WithMessage("O quadro é obrigatório.");

        RuleFor(command => command.Name)
            .NotEmpty()
            .WithMessage("O nome da coluna é obrigatório.")
            .MaximumLength(100)
            .WithMessage("O nome da coluna não pode ter mais de 100 caracteres.");
    }
}
