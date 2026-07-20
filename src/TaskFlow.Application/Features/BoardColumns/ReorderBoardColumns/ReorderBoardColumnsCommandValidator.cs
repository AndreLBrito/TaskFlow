using FluentValidation;

namespace TaskFlow.Application.Features.BoardColumns.ReorderBoardColumns;

public class ReorderBoardColumnsCommandValidator
    : AbstractValidator<ReorderBoardColumnsCommand>
{
    public ReorderBoardColumnsCommandValidator()
    {
        RuleFor(command => command.BoardId)
            .NotEmpty()
            .WithMessage("O quadro é obrigatório.");

        RuleFor(command => command.ColumnIds)
            .Cascade(CascadeMode.Stop)
            .NotEmpty()
            .WithMessage("Informe a ordem das colunas.")
            .Must(columnIds => columnIds.Distinct().Count() == columnIds.Count)
            .WithMessage("A lista de colunas não pode conter IDs repetidos.");

        RuleForEach(command => command.ColumnIds)
            .NotEmpty()
            .WithMessage("A coluna é obrigatória.");
    }
}
