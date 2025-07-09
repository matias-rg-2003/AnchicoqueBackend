namespace ProductService.Validations;

using FluentValidation;
using ProductService.Dto.InDto;

public class LineLeatherAssociationValidator : AbstractValidator<LineLeatherAssociationDTO>
{
    public LineLeatherAssociationValidator()
    {
        RuleFor(dto => dto.IdLine).NotEmpty();
        RuleFor(dto => dto.LeathersIds).NotEmpty().WithMessage("At least one product ID must be provided");
    }
}
