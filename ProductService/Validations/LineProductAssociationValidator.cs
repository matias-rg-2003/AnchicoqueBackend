namespace ProductService.Validations;

using FluentValidation;
using ProductService.Dto.InDto;

public class LineProductAssociationValidator : AbstractValidator<LineProductAssociationDTO>
{
    public LineProductAssociationValidator()
    {
        RuleFor(dto => dto.IdLine).NotEmpty();
        RuleFor(dto => dto.ProductsIds).NotEmpty().WithMessage("At least one product ID must be provided");
    }
}
