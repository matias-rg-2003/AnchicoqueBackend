namespace ProductService.Validations;

using ProductService.Dto.InDto;
using FluentValidation;

public class ProductUpdateValidator : AbstractValidator<ProductUpdateDTO>
{
    public ProductUpdateValidator()
    {
        RuleFor(product => product.Id).NotEmpty().WithMessage("Id required to update");
        RuleFor(product => product.Name).MaximumLength(64);
        RuleFor(product => product.Description).MinimumLength(500);
    }
}
