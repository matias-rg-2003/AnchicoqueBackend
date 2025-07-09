namespace ProductService.Validations;

using FluentValidation;
using ProductService.Dto.InDto;

public class ProductValidator : AbstractValidator<ProductCreateDTO>
{
    public ProductValidator()
    {
        RuleFor(product => product.Name).NotEmpty().MaximumLength(64);
        RuleFor(product => product.Description).MaximumLength(500);
        RuleFor(product => product.Height).NotEmpty();
        RuleFor(product => product.Width).NotEmpty();
        RuleFor(product => product.Depth).NotEmpty();
    }
}
